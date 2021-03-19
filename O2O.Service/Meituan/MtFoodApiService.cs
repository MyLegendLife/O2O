using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO.Api;
using System;
using System.Collections.Generic;
using System.Linq;

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
                                Stock = s["stock"].ToString() == "" ? "9999" : s["stock"],
                                State = d["is_sold_out"]
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
            //var listFoodData = new List<FoodData>();

            //var codes = data.GroupBy(a => a["ProdCode"]);
            //foreach (var code in codes)
            //{
            //    var foodData = new FoodData() { app_food_code = code.Key.ToString() };

            //    var skus = data.Where(a => a["ProdCode"] == code.Key);

            //    var listSku = new List<Sku>();
            //    foreach (var sku in skus)
            //    {
            //        listSku.Add(new Sku() { sku_id = sku["ProdNo"].ToString(), stock = sku["Stock"].ToString() });
            //    }

            //    foodData.skus = listSku;
            //    listFoodData.Add(foodData);
            //}

            //var model = new
            //{
            //    timestamp = ToolsCommon.GetTimestamp().ToString(),
            //    app_id = _waimaiAppId,
            //    app_poi_code = _shopNo,
            //    food_data = JsonConvert.SerializeObject(listFoodData)
            //};

            //var modelBody = new
            //{
            //    app_poi_code = _shopNo,
            //    food_data = JsonConvert.SerializeObject(listFoodData)
            //};

            //string url = GetUrl("https://waimaiopen.meituan.com/api/v1/food/sku/stock", model);

            //var res = HttpCommon.Post(url, modelBody);

            //return Tools.ResultMt(res);

            int num1 = data.Count<JToken>();
            int size = 50;
            int num2 = size;
            int num3 = (num1 + num2 - 1) / size;
            string msg = "";
            for (int index = 0; index < num3; ++index)
            {
                IEnumerable<JToken> source = data.Skip<JToken>(index * size).Take<JToken>(size);
                System.Collections.Generic.List<FoodData> foodDataList = new System.Collections.Generic.List<FoodData>();
                foreach (IGrouping<JToken, JToken> grouping in source.GroupBy<JToken, JToken>((Func<JToken, JToken>)(a => a[(object)"ProdCode"])))
                {
                    IGrouping<JToken, JToken> code = grouping;
                    FoodData foodData = new FoodData()
                    {
                        app_food_code = code.Key.ToString()
                    };
                    IEnumerable<JToken> jtokens = source.Where<JToken>((Func<JToken, bool>)(a => a[(object)"ProdCode"] == code.Key));
                    System.Collections.Generic.List<Sku> skuList = new System.Collections.Generic.List<Sku>();
                    foreach (JToken jtoken in jtokens)
                        skuList.Add(new Sku()
                        {
                            sku_id = jtoken[(object)"ProdNo"].ToString(),
                            stock = jtoken[(object)"Stock"].ToString()
                        });
                    foodData.skus = (ICollection<Sku>)skuList;
                    foodDataList.Add(foodData);
                }
                var data1 = new
                {
                    timestamp = ToolsCommon.GetTimestamp().ToString(),
                    app_id = this._waimaiAppId,
                    app_poi_code = this._shopNo,
                    food_data = JsonConvert.SerializeObject((object)foodDataList)
                };
                var data2 = new
                {
                    app_poi_code = this._shopNo,
                    food_data = JsonConvert.SerializeObject((object)foodDataList)
                };
                Result result = Tools.ResultMt(HttpCommon.Post(this.GetUrl("https://waimaiopen.meituan.com/api/v1/food/sku/stock", (object)data1), (object)data2));
                msg += result.Msg;
            }
            return string.IsNullOrEmpty(msg) ? Tools.ResultOk() : Tools.ResultErr(msg);
        }

        //可能数据过多超出请求长度，分批次处理
        public Result UpdateState(int state, IEnumerable<JToken> data)
        {
            var result = new Result() { State = "OK" };

            var size = 50;
            var loop = (data.Count() + size - 1) / size;

            for (var i = 0; i < loop; i++)
            {
                var res = UpdateStateSingle(state, data.Skip(i * size).Take(size));

                if (res.State == "ERR")
                {
                    result.State = res.State;
                    result.Msg += res.Msg;
                }
            }

            return result;
        }

        public Result UpdateStateSingle(int state, IEnumerable<JToken> data)
        {
            var foodDataSellStatusList = new List<FoodDataSellStatus>();
            foreach (var grouping in data.GroupBy(a => a["ProdCode"]))
            {
                var code = grouping;
                var foodDataSellStatus = new FoodDataSellStatus()
                {
                    app_food_code = code.Key.ToString()
                };
                var tokens = data.Where(a => a["ProdCode"] == code.Key);
                var skuSellStatusList = new List<SkuSellStatus>();
                foreach (var token in tokens)
                {
                    skuSellStatusList.Add(new SkuSellStatus()
                    {
                        sku_id = token["ProdNo"].ToString()
                    });
                }

                foodDataSellStatus.skus = skuSellStatusList;
                foodDataSellStatusList.Add(foodDataSellStatus);
            }
            var data1 = new
            {
                timestamp = ToolsCommon.GetTimestamp().ToString(),
                app_id = _waimaiAppId,
                app_poi_code = _shopNo,
                food_data = JsonConvert.SerializeObject(foodDataSellStatusList),
                sell_status = state
            };
            var data2 = new
            {
                app_poi_code = _shopNo,
                food_data = JsonConvert.SerializeObject(foodDataSellStatusList),
                sell_status = state
            };
            return Tools.ResultMt(HttpCommon.Post(GetUrl("https://waimaiopen.meituan.com/api/v1/food/sku/sellStatus", data1), data2));
        }
    }
}