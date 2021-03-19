using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace O2O.Web.Areas.Meituan.Controllers
{
    public class ShopMapController : Controller
    {
        private readonly string _tuangouAppKey;
        private readonly string _tuangouAppSecret;

        public ShopMapController()
        {
            IMtAccountService service = new MtAccountService();
            Mt_AccountDTO account = service.GetAccount(Global.USER_ID);

            _tuangouAppKey = account.TuangouAppKey;
            _tuangouAppSecret = account.TuangouAppSecret;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var shopList = Bak365Service.GetAreaShopList();

            shopList = QueryMapped(shopList);

            return View(shopList);
        }

        [HttpPost]
        public ActionResult GetUrl(string shopNo, string shopName,bool isMap)
        {
            var basicUrl = isMap ? "https://open-api.dianping.com/auth/shopMapping" : "https://open-api.dianping.com/auth/shopMapping/delete";
            if (isMap)
            {
                var model = new
                {
                    vendorShopId = shopNo,
                    vendorShopName = shopName,
                    businessType = "101",
                    appKey = _tuangouAppKey,
                    ts = ToolsCommon.GetTimestamp(),
                    version = "2.0",
                };

                string url = CreateUrl(basicUrl, model, null) ;

                return Json(new AjaxResult() { state = "ok", msg = url });
            }
            else
            {
                var model = new
                {
                    vendorShopId = shopNo,
                    businessType = "101",
                    appKey = _tuangouAppKey,
                    ts = ToolsCommon.GetTimestamp(),
                    version = "2.0",
                };

                string url = CreateUrl(basicUrl, model, null);

                return Json(new AjaxResult() { state = "ok", msg = url });
            }
        }

        public List<ShopInfo> QueryMapped(List<ShopInfo> list)
        {
            int x = 49;
            int y = list.Count() / x + 1;

            Parallel.For(0, y, i => {
                var modelSys = new
                {
                    appKey = _tuangouAppKey,
                    ts = ToolsCommon.GetTimestamp(),
                    version = "2.0"
                };

                List<string> vendorShopIdList = new List<string>();
                for (int j = i * x; j < (i + 1) * x; j++)
                {
                    if (j >= list.Count()) break;
                    vendorShopIdList.Add(list[j].ShopNo);
                }
                JArray dpShopIdList = new JArray();
                var model = new
                {
                    dpShopIdList = dpShopIdList,
                    vendorShopIdList = vendorShopIdList.ToArray(),
                    businessType = 101
                };

                string url = CreateUrl("https://open-api.dianping.com/vendor/shopMapping/query", modelSys, model);
                string result = HttpCommon.Post(url, model);

                JObject json = JObject.Parse(result);

                if (json["code"].ToString() == "200")
                {
                    JArray jArray = JArray.Parse(json["content"].ToString());
                    for (int k = 0; k < jArray.Count; k++)
                    {
                        var shopInfo = list.FirstOrDefault(a => a.ShopNo == (jArray[k]["vendorShopId"].ToString()));
                        if (shopInfo != null)
                            shopInfo.IsMapped = true;
                    }
                }
            });

            #region
            //for (int i = 0; i < y; i++)
            //{
            //    var modelSys = new {
            //        appKey = _tuangouAppKey,
            //        ts = ToolsCommon.GetTimestamp(),
            //        version = "2.0"
            //    };

            //    List<string> vendorShopIdList = new List<string>();
            //    for (int j = i * x; j < (i + 1) * x; j++)
            //    {
            //        if (j >= list.Count()) break;
            //        vendorShopIdList.Add(list[j].ShopNo);
            //    }
            //    JArray dpShopIdList = new JArray();
            //    var model = new {
            //        dpShopIdList= dpShopIdList,
            //        vendorShopIdList= vendorShopIdList.ToArray(),
            //        businessType = 101
            //    };

            //    string url = CreateUrl("https://open-api.dianping.com/vendor/shopMapping/query", modelSys, model);
            //    string result = HttpCommon.Post(url, model);

            //    JObject json = JObject.Parse(result);

            //    if (json["code"].ToString() == "200")
            //    {
            //        JArray jArray = JArray.Parse(json["content"].ToString());
            //        for (int k = 0; k < jArray.Count; k++)
            //        {
            //            var shopInfo = list.FirstOrDefault(a => a.ShopNo == (jArray[k]["vendorShopId"].ToString()));
            //            if (shopInfo != null)
            //                shopInfo.IsMapped = true;
            //        }
            //    }
            //}
            #endregion

            return list;
        }

        public string CreateUrl(string url, object modelSys, object model)
        {
            var sort = modelSys.GetType().GetProperties().OrderBy(a => a.Name);

            string str = "";
            string pas = "";
            foreach (var item in sort)
            {
                str += item.Name + item.GetValue(modelSys, null);
                pas += item.Name + "=" + item.GetValue(modelSys, null) + "&";
            }

            string modelStr = model is null ? "" : JsonConvert.SerializeObject(model);
            str = _tuangouAppSecret + str + modelStr;

            string sign = ToolsCommon.SHA1Encrypt(str);

            return url + "?" + pas + "sign=" + sign;
        }
    }
}