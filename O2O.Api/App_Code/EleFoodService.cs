using Newtonsoft.Json.Linq;
using O2O.Common;
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
                                    ProdCode = spec["specId"],
                                    ProdNo = spec["extendCode"],
                                    ProdName = itemName + spec["name"],
                                    Spec = "",
                                    Price = spec["price"],
                                    Stock = spec["stock"]
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

                Dictionary<string, int> dic = new Dictionary<string, int>();

                foreach (var item in dd)
                {
                    int stock = 0;
                    if (ToolsCommon.IsPositiveNumber(item["Stock"].ToString()))
                    {
                        stock = (int)item["Stock"];
                    }
                    dic.Add(item["ProdCode"].ToString(), stock);
                }

                string res = Good(shop.AccessToken, dic);
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

        public string Good(string token, Dictionary<string, int> dic)
        {
            var res = _foodApiService.BatchUpdateStock(token, dic);

            if (res.error != null)
            {
                return "ERR," + res.error.message;
            }
            else
            {
                JObject joRes = JObject.Parse(res.result.ToString());
                if (((JContainer)joRes["failures"]).Count > 0)
                {
                    string msg = "";
                    JArray jaRes = JArray.Parse(joRes["failures"].ToString());
                    foreach (var i in jaRes)
                    {
                        msg += i["id"] + ":" + i["description"] + ";";
                    }

                    return msg;
                }
            }

            return "";
        }
    }
}