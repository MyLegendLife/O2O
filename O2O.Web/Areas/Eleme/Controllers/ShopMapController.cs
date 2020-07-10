using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Service;
using O2O.Service.Eleme;
using O2O.Web.App_Start;
using O2O.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace O2O.Web.Areas.Eleme.Controllers
{
    public class ShopMapController : Controller
    {
        public IEleAccountService _serviceAccount { get; set; }
        public IEleShopService _serviceShop { get; set; }

        [HttpGet]
        public ActionResult List()
        {
            var list = GetList();

            return View(list);
        }

        [HttpPost]
        public ActionResult List(string type)
        {
            var list = GetList(type);

            return View(list);
        }

        public List<ShopMap> GetList(string type = "")
        {
            var list = new List<ShopMap>();

            //获取账户
            var accounts = _serviceAccount.GetAccounts(Global.USER_ID);

            //已经映射过的门店
            var listMapped = _serviceShop.GetByUserId(Global.USER_ID);

            //获取365门店
            var listShop365 = Bak365Service.GetShopList();

            //根据账户获取门店
            foreach (var account in accounts)
            {
                EleUserApiService serviceUser = new EleUserApiService();
                var res = serviceUser.GetUser(account.AccessToken);
                if (res.error != null) continue;

                JObject jObject = JObject.Parse(res.result.ToString());
                JArray shops = JArray.Parse(jObject["authorizedShops"].ToString());

                foreach (var shop in shops)
                {
                    ShopMap shopMap = new ShopMap();
                    shopMap.AccountId = account.Id;
                    shopMap.AccountNo = account.AccountNo;
                    shopMap.AccountName = account.AccountName;
                    shopMap.AccessToken = account.AccessToken;
                    shopMap.ShopId = (long)shop["id"];
                    shopMap.ShopNameEle = shop["name"].ToString();

                    if (type.ToUpper() == "MATCH")
                    {
                        foreach (var shop365 in listShop365)
                        {
                            if (shop["name"].ToString().Contains(shop365.Value))
                            {
                                shopMap.ShopNo = shop365.Key;
                                shopMap.ShopName = shop365.Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var dd = listMapped.Find(a => a.ShopId.ToString() == shop["id"].ToString());
                        if (dd != null)
                        {
                            shopMap.ShopNo = dd.ShopNo;
                            shopMap.ShopName = listShop365[dd.ShopNo];
                        }
                    }

                    list.Add(shopMap);
                }
            }

            return list;
        }

        [HttpPost]
        public ActionResult GetShop(string shopNo)
        {
            var shop = Bak365Service.GetShopInfo(shopNo);

            if (shop == null)
            {
                return Json(new AjaxResult() { state = "no", msg = "无此门店" });
            }
            else
            {
                return Json(new AjaxResult() { state = "ok", data = shop });
            }
        }

        public ActionResult Map()
        {
            var stream = new StreamReader(Request.InputStream);
            var str = stream.ReadToEnd();

            var list = JsonConvert.DeserializeObject<List<ShopMap>>(str);

            EleShopApiService service = new EleShopApiService();

            string msg = "";
            foreach (var item in list)
            {
                var obj = new
                {
                    openId = item.ShopNo
                };

                var res = service.UpdateShop(item.AccessToken, item.ShopId, obj);

                if (res.error == null)
                {
                    //保存门店到数据库
                    var dto = _serviceShop.GetByShopId(item.ShopId);
                    if (dto == null)
                    {
                        Ele_ShopDTO dtoNew = new Ele_ShopDTO()
                        {
                            AccountId = item.AccountId,
                            ShopId = item.ShopId,
                            ShopNo = item.ShopNo,
                        };
                        _serviceShop.Add(dtoNew);
                    }
                    else
                    {
                        dto.ShopNo = item.ShopNo;
                        _serviceShop.Update(dto);
                    }
                }
                else
                {
                    msg = "【" + item.ShopId + "】" + res.error.message;
                }
            }

            if (msg != "")
            {
                return Json(new AjaxResult() { state = "no", msg = msg });
            }
            else
            {
                return Json(new AjaxResult() { state = "ok" });
            }
        }

        public ActionResult Cancel(string token, long shopId)
        {
            EleShopApiService service = new EleShopApiService();

            var obj = new
            {
                openId = ""
            };

            var res = service.UpdateShop(token, shopId, obj);

            if (res.error == null)
            {
                _serviceShop.Delete(shopId);

                return Json(new AjaxResult() { state = "ok" });
            }
            else
            {
                return Json(new AjaxResult() { state = "no", msg = res.error.message });
            }
        }
    }
}