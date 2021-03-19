using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.BackgroundJobs.Models;
using O2O.Common;
using O2O.DTO.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace O2O.BackgroundJobs.Services
{
    public class JobService : IJobService
    {
        public async Task MtUpdateState(string waimaiAppId, string waimaiAppSecret, string shopNo, int state, IEnumerable<StockModel> models)
        {
            var data = models.Select(item => new FoodDataSellStatus()
            {
                app_food_code = item.ProdNo,
                skus = new List<SkuSellStatus>()
                {
                    new SkuSellStatus()
                        {sku_id = item.ProdNo}
                }
            });

            //分批次提交
            var size = 50;
            var loop = (data.Count() + size - 1) / size;
            for (var j = 0; j < loop; j++)
            {
                var batchFoods = data.Skip(j * size).Take(size);

                var model = new JObject()
                {
                    {"timestamp" , ToolsCommon.GetTimestamp().ToString()},
                    {"app_id" , waimaiAppId},
                    {"app_poi_code" , shopNo},
                    {"food_data" , JsonConvert.SerializeObject(batchFoods)},
                    {"sell_status" , state}
                };

                var url = GetUrl(waimaiAppSecret, "https://waimaiopen.meituan.com/api/v1/food/sku/sellStatus", model);

                model.Remove("timestamp");
                model.Remove("app_id");
                var response = await HttpCommon.PostJObjectAsync(url, model);
            }
        }

        public async Task MtUpdateStock(string waimaiAppId, string waimaiAppSecret, string shopNo, IEnumerable<StockModel> models)
        {
            var size = 50;
            var loop = (models.Count() + size - 1) / size;
            for (var i = 0; i < loop; ++i)
            {
                var data = models.Skip(i * size).Take(size);
                var foodDataList = new List<FoodData>();
                foreach (var grouping in data.GroupBy(a => a.ProdCode))
                {
                    var code = grouping;
                    var foodData = new FoodData()
                    {
                        app_food_code = code.Key
                    };

                    foodData.skus = data
                        .Where(a => a.ProdCode == code.Key)
                        .Select(token => new Sku() { sku_id = token.ProdNo, stock = token.CurrentStock.ToString() })
                        .ToList();

                    foodDataList.Add(foodData);
                }

                var model = new JObject()
                {
                    {"timestamp" , ToolsCommon.GetTimestamp().ToString()},
                    {"app_id" , waimaiAppId},
                    {"app_poi_code" , shopNo},
                    {"food_data" , JsonConvert.SerializeObject(foodDataList)}
                };

                var url = GetUrl(waimaiAppSecret, "https://waimaiopen.meituan.com/api/v1/food/sku/stock", model);
                model.Remove("timestamp");
                model.Remove("app_id");

                var response = await HttpCommon.PostJObjectAsync(url, model);
            }
        }

        public async Task<IEnumerable<StockModel>> MtFoods(string waimaiAppId, string waimaiAppSecret, string shopNo)
        {
            var model = new JObject()
            {
                {"timestamp" ,ToolsCommon.GetTimestamp()},
                {"app_id" ,waimaiAppId},
                {"app_poi_code", shopNo}
            };

            var url = GetUrl(waimaiAppSecret, "https://waimaiopen.meituan.com/api/v1/food/list", model);
            var res = HttpCommon.Get(url);

            var json = JObject.Parse(res);

            if (json["error"] != null) return new List<StockModel>();

            var data = JArray.Parse(json["data"].ToString());

            var list = new List<StockModel>();
            foreach (var d in data)
            {
                var skus = JArray.Parse(d["skus"].ToString());
                foreach (var s in skus)
                {
                    if (d["app_food_code"] != null & s["sku_id"] != null)
                    {
                        if (d["app_food_code"].ToString() != "" && s["sku_id"].ToString() != "")
                        {
                            var prod = new StockModel()
                            {
                                ProdCode = d["app_food_code"].ToString(),
                                ProdNo = s["sku_id"].ToString(),
                            };

                            list.Add(prod);
                        }
                    }
                }
            }

            return list;
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