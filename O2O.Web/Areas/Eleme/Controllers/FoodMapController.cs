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
using System.Web.Http.Results;
using System.Web.Mvc;

namespace O2O.Web.Areas.Eleme.Controllers
{
    public class FoodMapController : Controller
    {
        public IEleAccountService _serviceAccount { get; set; }
        public IEleShopService _serviceShop { get; set; }
        private EleFoodApiService _serviceFood { get; set; }
        private List<ProdInfo> _prodList { get; set; }

        public FoodMapController()
        {
            _serviceFood = new EleFoodApiService();
            _prodList = Bak365Service.GetProdList();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(string token, long shopId)
        {
            var list = GetFoodMapList(token, shopId);

            return PartialView("List", list);
        }

        public ActionResult GetProd(string[] prodNos)
        {
            var prods = from m in _prodList
                        join n in prodNos on m.ProdNo equals n
                        select m;

            return Json(new AjaxResult() { state = "ok", data = prods });
        }

        /// <summary>
        /// 给页面提供json格式的节点数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Tree()
        {
            //获取账户
            var accounts = _serviceAccount.GetAccounts(Global.USER_ID);

            var nodes = new List<ZTreeNode>();
            foreach (var account in accounts)
            {
                nodes.Add(new ZTreeNode
                {
                    id = account.Id.ToString(),
                    name = account.AccountName,
                    pId = "",
                    isParent = true
                });

                var serviceUser = new EleUserApiService();
                var res = serviceUser.GetUser(account.AccessToken);
                if (res.error != null) continue;

                var jObject = JObject.Parse(res.result.ToString());
                var shops = JArray.Parse(jObject["authorizedShops"].ToString());

                foreach (var shop in shops)
                {
                    nodes.Add(new ZTreeNode
                    {
                        id = shop["id"].ToString(),
                        name = shop["name"].ToString(),
                        pId = account.Id.ToString(),
                        token = account.AccessToken
                    });
                }
            }

            //将获取的节点集合转换为json格式字符串，并返回
            var json = JsonConvert.SerializeObject(nodes);
            return json;
        }

        [HttpPost]
        public ActionResult Match()
        {
            var stream = new StreamReader(Request.InputStream);
            var str = stream.ReadToEnd();

            JArray data = JArray.Parse(str);

            foreach (var item in data)
            {
                var prod = _prodList.Find(a => (a.ProdName.Contains(item["ItemName"].ToString()) || item["ItemName"].ToString().Contains(a.ProdName)) && a.Price.ToString() == item["ItemPrice"].ToString());
                if (prod != null)
                {
                    item["ProdNo"] = prod.ProdNo;
                    item["ProdName"] = prod.ProdName;
                    item["ProdUnit"] = prod.ProdUnit;
                    item["Price"] = prod.Price;
                }
                //else
                //{
                //    item["ProdNo"] = "";
                //    item["ProdName"] = "";
                //    item["ProdUnit"] = "";
                //    item["Price"] = "";
                //}
            }
            string json = JsonConvert.SerializeObject(data);

            return Json(new AjaxResult() { state = "ok", data = json });
        }

        public ActionResult Map()
        {
            var stream = new StreamReader(Request.InputStream);
            var str = stream.ReadToEnd();

            var list = JsonConvert.DeserializeObject<List<FoodMap>>(str);

            string msg = "";
            foreach (var item in list)
            {
                string res = DoMap(item.Token,item.ItemId,item.SpecId,item.ProdNo);
                if (!string.IsNullOrEmpty(res))
                {
                    msg += res ;
                }
            }

            if (string.IsNullOrEmpty(msg))
            {
                return Json(new AjaxResult() { state="ok" });
            }
            else
            {
                return Json(new AjaxResult() { state = "no", msg = msg });
            }
        }

        public string DoMap(string token, long itemId, long specId, string prodNo)
        {
            var resItem = _serviceFood.GetItem(token, itemId);

            if (resItem.error == null)
            {
                OItem item = JsonConvert.DeserializeObject<OItem>(resItem.result.ToString());

                foreach (var spec in item.specs)
                {
                    if (spec.specId == specId)
                    {
                        spec.extendCode = prodNo.Trim('X');
                        break;
                    }
                }

                OProduct product = new OProduct();
                product.itemId = itemId;
                product.categoryId = item.categoryId;
                product.properties.name = item.name;
                product.properties.specs = item.specs;
                product.properties.materials = item.materials;


                var resUpdate = _serviceFood.UpdateItem(token, product);

                if (resUpdate.error == null)
                {

                }
                else
                {
                    return "【" + specId + "】" + resUpdate.error.message;
                }
            }
            else
            {
                return "【" + specId + "】" + resItem.error.message;
            }

            return "";
        }

        [HttpGet]
        public FileResult ExcelExport(string token, long shopId)
        {
            var list = GetFoodMapList(token, shopId);

            string[] columns = { "Token", "CateId", "CateName", "ItemId", "ItemName", "SpecId", "SpecName", "Price", "Price", "ProdNo", "ProdName", "ProdUnit", "SalePrice" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(list, "", false, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "商品映射信息.xlsx");
        }

        [HttpPost]
        public ActionResult ExcelImport(HttpPostedFileBase file)
        {
            var list = ExcelExportHelper.ImportExcel<FoodMap>(file.InputStream);

            string msg = "";
            foreach (var item in list)
            {
                string res = DoMap(item.Token, item.ItemId, item.SpecId, item.ProdNo);
                if (!string.IsNullOrEmpty(res))
                {
                    msg += res;
                }
            }

            if (string.IsNullOrEmpty(msg))
            {
                return Json(new AjaxResult() { state = "ok" });
            }
            else
            {
                return Json(new AjaxResult() { state = "no", msg = msg });
            }
        }

        public List<FoodMap> GetFoodMapList(string token,long shopId)
        {
            var list = new List<FoodMap>();

            var resCate = _serviceFood.GetShopCategories(token, shopId);

            if (resCate.error == null)
            {
                var cates = JsonConvert.DeserializeObject<List<OCategory>>(resCate.result.ToString());

                foreach (var cate in cates)
                {
                    var resItem = _serviceFood.GetItemsByCategoryId(token, cate.id);

                    if (resItem.error != null) continue;

                    var dics = JsonConvert.DeserializeObject<Dictionary<long, OItem>>(resItem.result.ToString());

                    foreach (var dic in dics)
                    {
                        var item = dic.Value;

                        foreach (var spec in item.specs)
                        {
                            var foodMap = new FoodMap();

                            foodMap.Token = token;
                            foodMap.CateId = cate.id;
                            foodMap.CateName = cate.name;
                            foodMap.ItemId = item.id;
                            foodMap.ItemName = item.name;
                            foodMap.SpecId = spec.specId;
                            foodMap.SpecName = spec.name;
                            foodMap.Price = spec.price;
                            foodMap.Stock = spec.stock;

                            var extendCode = spec.extendCode;
                            var codes = extendCode.Split(new[] { 'X' }, StringSplitOptions.RemoveEmptyEntries);

                            var prodNo = "";
                            var prodName = "";
                            var prodUnit = "";
                            var price = "";
                            foreach (var code in codes)
                            {
                                var prod = _prodList.Find(a => a.ProdNo == code);
                                if (prod != null)
                                {
                                    prodNo += $"{prod.ProdNo}\r\n";
                                    prodName += $"{prod.ProdName}\r\n";
                                    prodUnit += $"{prod.ProdUnit}\r\n";
                                    price += $"{prod.Price}\r\n";
                                }
                            }

                            foodMap.ProdNo = prodNo.Trim("\r\n".ToCharArray());
                            foodMap.ProdName = prodName.Trim("\r\n".ToCharArray());
                            foodMap.ProdUnit = prodUnit.Trim("\r\n".ToCharArray());
                            foodMap.SalePrice = price.Trim("\r\n".ToCharArray());

                            list.Add(foodMap);
                        }
                    }
                }
            }

            return list;
        }
    }
}