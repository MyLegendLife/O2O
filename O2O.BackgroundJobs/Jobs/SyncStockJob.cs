using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.BackgroundJobs.Utils;
using O2O.Common;
using O2O.DTO;
using O2O.DTO.Api;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Service.Eleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using O2O.BackgroundJobs.Models;
using O2O.Service.Meituan;

namespace O2O.BackgroundJobs.Jobs
{
    public class SyncStockJob
    {
        private readonly IUserService _userService;
        private readonly IShopConfigService _shopConfigService;
        public readonly IMtAccountService _accountService;
        public readonly IEleShopService _eleShopService;
        private static ILog _log = LogManager.GetLogger("SyncStockJob");

        public SyncStockJob(IUserService userService, IEleShopService eleShopService, IShopConfigService shopConfigService, IMtAccountService accountService)
        {
            _userService = userService;
            _eleShopService = eleShopService;
            _shopConfigService = shopConfigService;
            _accountService = accountService;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var users = _userService.GetAll();

                foreach (var user in users)
                {
                    await MtSyncStock(user);
                    await EleSyncStock(user);
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat($"【系统错误】：{e.Message}");
            }
        }

        public async Task MtSyncStock(UserDTO user)
        {
            var bak365Util = new Bak365Util(user.ConnString);

            //获取美团开启自动同步库存的门店
            var shopConfigs = await _shopConfigService.GetAutoSyncListAsync(user.Id, 0);

            if (shopConfigs.Count > 0)
            {
                var account = _accountService.GetAccount(user.Id);

                foreach (var shopConfig in shopConfigs)
                {
                    //获取门店商品
                    var foods = GetMtFoods(account, shopConfig.ShopNo);

                    //获取365库存
                    var currentStocks =
                        bak365Util.GetCurrentStock(shopConfig.ShopNo, (IEnumerable<string>)foods.Select(x => x.ProdNo));

                    var data = from a in foods
                        join b in currentStocks on a.ProdNo equals b.ProdNo
                        select new StockModel()
                        {
                            ProdCode = a.ProdCode,
                            ProdNo = a.ProdNo,
                            Stock = b.Stock
                        };

                    //同步库存
                    MtUpdateStock(account, shopConfig.ShopNo, data);
                }
            }
        }

        public async Task EleSyncStock(UserDTO user)
        {
            var bak365Util = new Bak365Util(user.ConnString);

            //获取美团开启自动同步库存的门店
            var shopConfigs = await _shopConfigService.GetAutoSyncListAsync(user.Id, 0);

            if (shopConfigs.Count > 0)
            {
                var account = _accountService.GetAccount(user.Id);

                foreach (var shopConfig in shopConfigs)
                {
                    //获取门店商品
                    var foods = GetMtFoods(account, shopConfig.ShopNo);

                    //获取365库存
                    var currentStocks =
                        bak365Util.GetCurrentStock(shopConfig.ShopNo, (IEnumerable<string>)foods.Select(x => x.ProdNo));

                    var data = from a in foods
                        join b in currentStocks on a.ProdNo equals b.ProdNo
                        select new
                        {
                            ProdCate = a.ProdCode,
                            ProdNo = a.ProdNo,
                            Stock = b.Stock
                        };
 
                    //同步库存
                    MtUpdateStock(account,shopConfig.ShopNo,data);
                }
            }
        }

        public IEnumerable<dynamic> GetMtFoods(Mt_AccountDTO account,string shopNo)
        {
            var model = new JObject()
            {
                {"timestamp" ,ToolsCommon.GetTimestamp()},
                {"app_id" ,account.WaimaiAppId},
                {"app_poi_code", shopNo}
            };

            var url = GetUrl(account.WaimaiAppSecret, "https://waimaiopen.meituan.com/api/v1/food/list", model);
            var res = HttpCommon.Get(url);

            var json = JObject.Parse(res);

            if (json["error"] != null) return new List<dynamic>();

            var data = JArray.Parse(json["data"].ToString());

            var list = new List<dynamic>();
            foreach (var d in data)
            {
                var skus = JArray.Parse(d["skus"].ToString());
                foreach (var s in skus)
                {
                    if (d["app_food_code"] != null & s["sku_id"] != null)
                    {
                        if (d["app_food_code"].ToString() != "" && s["sku_id"].ToString() != "")
                        {
                            var prod = new
                            {
                                CateName = d["category_name"],
                                ProdCode = d["app_food_code"],
                                ProdNo = s["sku_id"],
                                ProdName = d["name"],
                                Spec = s["spec"],
                                Price = s["price"],
                                Stock = s["stock"].ToString() == "" ? "9999" : s["stock"],
                                State = d["is_sold_out"]
                            };

                            list.Add(prod);
                        }
                    }
                }
            }

            return list;
        }

        public void MtUpdateStock(Mt_AccountDTO account, string shopNo, IEnumerable<StockModel> data)
        {
            var num1 = data.Count();
            var size = 50;
            var num2 = size;
            var num3 = (num1 + num2 - 1) / size;
            var msg = "";
            for (var index = 0; index < num3; ++index)
            {
                var source = data.Skip(index * size).Take(size);
                var foodDataList = new List<FoodData>();
                foreach (var grouping in source.GroupBy(a => a["ProdCode"]))
                {
                    var code = grouping;
                    var foodData = new FoodData()
                    {
                        app_food_code = code.Key.ToString()
                    };
                    var tokens = source.Where(a => a["ProdCode"] == code.Key);
                    var skuList = new List<Sku>();
                    foreach (var jtoken in tokens)
                        skuList.Add(new Sku()
                        {
                            sku_id = jtoken["ProdNo"]?.ToString(),
                            stock = jtoken["Stock"]?.ToString()
                        });
                    foodData.skus = skuList;
                    foodDataList.Add(foodData);
                }
                var model = new JObject()
                {
                    {"timestamp" , ToolsCommon.GetTimestamp().ToString()},
                    {"app_id" , account.WaimaiAppId},
                    {"app_poi_code" , shopNo},
                    {"food_data" , JsonConvert.SerializeObject(foodDataList)}
                };

                var url = GetUrl(account.WaimaiAppSecret, "https://waimaiopen.meituan.com/api/v1/food/sku/stock", model);
                model.Remove("timestamp");
                model.Remove("app_id");

                var response = HttpCommon.PostJObject(url, model);
            }
        }

        public void EleUpdateStock(string userId, string shopNo, IEnumerable<dynamic> data)
        {
            var service = new EleFoodApiService();

            var shop = _eleShopService.Get(userId, shopNo);
            if (shop is null) return;

            var total = data.Count();
            var size = 50;
            var pageCount = (total + size - 1) / size;

            var msg = "";
            for (var i = 0; i < pageCount; i++)
            {
                var dd = data.Skip(i * size).Take(size);

                var dic = new Dictionary<long, int>();

                foreach (var item in dd)
                {
                    var stock = 0;
                    if (ToolsCommon.IsPositiveNumber(item["Stock"].ToString()))
                    {
                        stock = (int)item["Stock"];
                    }
                    dic.Add(long.Parse(item["ProdCode"].ToString().Split(';')[1]), stock);
                }

                var result = service.BatchUpdateStock(shop.AccessToken, dic);
            }
        }

        public string GetUrl(string appSecret, string url, JObject model)
        {
            var sorted = new JObject(model.Properties().OrderBy(x => x.Name));

            var str = "";
            foreach (var item in sorted)
            {
                str += item.Key + "=" + item.Value + "&";
            }

            //var str = sort.Aggregate("", (current, item) => current + (item.Name + "=" + item.GetValue(model, null) + "&"));


            str = str.TrimEnd('&');

            var sig = ToolsCommon.MD5Encrypt(url + "?" + str + appSecret);

            return url + "?" + str + "&sig=" + sig;
        }
    }
}
