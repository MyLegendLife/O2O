using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO.Api;
using O2O.DTO.Meituan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace O2O.Service.Meituan
{
    public class MtFoodApiService : MtBaseApiService
    {
        public MtFoodApiService(string userNo, string shopNo) : base(userNo, shopNo)
        {

        }

        public Result List()
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                app_poi_code = _shopNo,
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/food/list", model);
            string res = HttpCommon.Get(url);

            JObject json = JObject.Parse(res);

            if (json["error"] != null) return Tools.ResultErr(json["error"]["msg"].ToString());

            JArray data = JArray.Parse(json["data"].ToString());
            if (data == null) return Tools.ResultErr();

            var list = new List<object>();
            foreach (var d in data)
            {
                JArray skus = JArray.Parse(d["skus"].ToString());
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
                                Stock = s["stock"].ToString() == "" ? "9999" : s["stock"]
                            };

                            list.Add(prod);
                        }
                    }
                }
            }

            return Tools.ResultOk(list);
        }

        public Result SkuStock(JArray data)
        {
            var listFoodData = new List<FoodData>();

            var codes = data.GroupBy(a => a["ProdCode"]);
            foreach (var code in codes)
            {
                var foodData = new FoodData() { app_food_code = code.Key.ToString() };

                var skus = data.Where(a => a["ProdCode"] == code.Key);

                var listSku = new List<Sku>();
                foreach (var sku in skus)
                {
                    listSku.Add(new Sku() { sku_id = sku["ProdNo"].ToString(), stock = sku["Stock"].ToString() });
                }

                foodData.skus = listSku;
                listFoodData.Add(foodData);
            }

            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp().ToString(),
                app_id = _waimaiAppId,
                app_poi_code = _shopNo,
                food_data = JsonConvert.SerializeObject(listFoodData)
            };

            var modelBody = new
            {
                app_poi_code = _shopNo,
                food_data = JsonConvert.SerializeObject(listFoodData)
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/food/sku/stock", model);

            var res = HttpCommon.Post(url, modelBody);

            return Tools.ResultMt(res);
        }
    }
}