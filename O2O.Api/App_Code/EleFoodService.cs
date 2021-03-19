using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Service;
using O2O.Service.Eleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.App_Code
{
    public class EleFoodService
    {
        public IEleShopService _shopService { get; set; }
        public EleFoodApiService _foodApiService { get; set; }

        public EleFoodService()
        {
            _shopService = new EleShopService();
            _foodApiService = new EleFoodApiService();
        }

        public Result List(string userId, string shopNo)
        {
            var shop = _shopService.Get(userId, shopNo);
            if (shop is null) return Tools.ResultErr("未找到此门店");

            var resCates = _foodApiService.GetShopCategories(shop.AccessToken, shop.ShopId);

            if (resCates.error != null) return Tools.ResultErr(resCates.error.message);

            JArray cates = resCates.result as JArray;

            List<object> list = new List<object>();
            foreach (var cate in cates)
            {
                long cateId = long.Parse(cate["id"].ToString());
                string cateName = cate["name"].ToString();

                var resItems = _foodApiService.GetItemsByCategoryId(shop.AccessToken, cateId);
                if (resItems.error != null) continue;

                JObject items = ((JObject)(resItems.result));

                foreach (var item in items)
                {
                    JArray specs = item.Value["specs"] as JArray;
                    string itemName = item.Value["name"].ToString();

                    foreach (var spec in specs)
                    {
                        if (spec["extendCode"] != null)
                        {
                            if (spec["extendCode"].ToString() != "")
                            {
                                var prod = new
                                {
                                    CateName = cateName,
                                    ProdCode = item.Value["id"]?.ToString() + ";" + spec["specId"]?.ToString(),
                                    ProdNo = spec["extendCode"],
                                    ProdName = itemName + spec["name"],
                                    Spec = "",
                                    Price = spec["price"],
                                    Stock = spec["stock"],
                                    State = item.Value["onShelf"].ToString() == "0" ? "1" : "0"
                                };

                                list.Add(prod);
                            }
                        }
                    }
                }
            }

            return Tools.ResultOk(list);
        }

        public Result SkuStock(string userId, string shopNo, JArray data)
        {
            var shop = _shopService.Get(userId, shopNo);
            if (shop is null) return Tools.ResultErr("未找到此门店");

            int total = data.Count();
            int size = 50;
            int pageCount = (total + size - 1) / size;

            string msg = "";
            for (int i = 0; i < pageCount; i++)
            {
                var dd = data.Skip(i * size).Take(size);

                var dic = new Dictionary<long, int>();

                foreach (var item in dd)
                {
                    int stock = 0;
                    if (ToolsCommon.IsPositiveNumber(item["Stock"].ToString()))
                    {
                        stock = (int)item["Stock"];
                    }
                    dic.Add(long.Parse(item["ProdCode"].ToString().Split(';')[1]), stock);
                }

                var result = _foodApiService.BatchUpdateStock(shop.AccessToken, dic);

                msg += this.Good(result);
            }

            if (string.IsNullOrEmpty(msg))
            {
                return Tools.ResultOk();
            }
            else
            {
                return Tools.ResultErr(msg);
            }
        }

        public Result UpdateState(string userId, string shopNo, int state, JArray data)
        {
            Ele_ShopDTO eleShopDto = _shopService.Get(userId, shopNo);
            if (eleShopDto == null)
                return Tools.ResultErr("未找到此门店");
            int num1 = data.Count<JToken>();
            int count = 50;
            int num2 = (num1 + count - 1) / count;
            string msg = "";
            for (int index = 0; index < num2; ++index)
            {
                IEnumerable<JToken> jtokens = data.Skip<JToken>(index * count).Take<JToken>(count);
                System.Collections.Generic.List<long> itemIds = new System.Collections.Generic.List<long>();
                foreach (JToken jtoken in jtokens)
                    itemIds.Add(long.Parse(jtoken[(object)"ProdCode"].ToString().Split(';')[0]));
                EleResult result = new EleResult();
                if (state == 0)
                {
                    result = _foodApiService.BatchListItems(eleShopDto.AccessToken, itemIds);
                }
                else if (state == 1)
                {
                    result = _foodApiService.BatchDelistItems(eleShopDto.AccessToken, itemIds);
                }
                msg += this.Good(result);
            }
            return string.IsNullOrEmpty(msg) ? Tools.ResultOk() : Tools.ResultErr(msg);
        }

        public string Good(EleResult result)
        {
            if (result.error != null)
            {
                return "ERR," + result.error.message;
            }

            JObject jobject = JObject.Parse(result.result.ToString());

            if (((JContainer)jobject["failures"]).Count <= 0) return "";

            string str = "";
            foreach (JToken jtoken in JArray.Parse(jobject["failures"].ToString()))
                str = str + jtoken["id"]?.ToString() + ":" + jtoken["description"]?.ToString() + ";";
            return str;
        }
    }
}