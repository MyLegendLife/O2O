using Newtonsoft.Json;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Service;
using O2O.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class StockRuleController : Controller
    {
        private readonly IStockRuleService _stockRuleService;

        public StockRuleController(IStockRuleService stockRuleService)
        {
            _stockRuleService = stockRuleService;
        }

        #region

        public async Task<ActionResult> RuleIndex()
        {
            var dtos = await _stockRuleService.GetListAsync(Global.USER_ID);

            return View(dtos);
        }

        [HttpGet]
        public async Task<ActionResult> RuleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RuleCreate(StockRuleDTO stockRule, HttpPostedFileBase file)
        {
            stockRule.UserId = Global.USER_ID;

            var list = ExcelExportHelper.ImportExcel<StockRuleProdDTO>(file.InputStream);

            await _stockRuleService.CreateAsync(stockRule, list);

            return Json(new AjaxResult() { state = "ok" });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            var list = await _stockRuleService.GetListShopAsync(id);

            if (list.Count > 0)
            {
                return Json(new AjaxResult() { state = "err", msg = "该模板有关联门店，请先解除关联" });
            }

            await _stockRuleService.DeleteAsync(id);

            return Json(new AjaxResult() { state = "ok" });
        }

        #endregion

        #region

        [HttpGet]
        public async Task<ActionResult> ProdIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> RuleTree()
        {
            //获取账户
            var rules = await _stockRuleService.GetListAsync(Global.USER_ID);

            var nodes = new List<ZTreeNode>();
            foreach (var rule in rules)
            {
                nodes.Add(new ZTreeNode
                {
                    id = rule.Id.ToString(),
                    name = rule.RuleName,
                    pId = ""
                });
            }

            //将获取的节点集合转换为json格式字符串，并返回
            var json = JsonConvert.SerializeObject(nodes);
            return json;
        }

        [HttpGet]
        public async Task<ActionResult> ProdList(Guid id)
        {
            ViewData["StockRuleId"] = id;

            var list = await _stockRuleService.GetListProdAsync(id);

            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> ProdCreate(Guid id)
        {
            ViewData["StockRuleId"] = id;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProdCreate(StockRuleProdDTO input)
        {
            //判断商品是否存在
            var dto = await _stockRuleService.GetProdAsync(input.StockRuleId, input.ProdNo);

            if (dto != null)
            {
                return Json(new AjaxResult() { state = "err", msg = "商品已经存在" });
            }

            await _stockRuleService.CreateProdAsync(input);

            return Json(new AjaxResult() { state = "ok" });
        }

        [HttpPost]
        public async Task<ActionResult> ProdDelete(Guid id)
        {
            await _stockRuleService.DeleteProdAsync(id);

            return Json(new AjaxResult() { state = "ok" });
        }

        [HttpPost]
        public async Task<ActionResult> GetProd(string prodNo)
        {
            var shop = await Task.FromResult(Bak365Service.GetProdInfo(prodNo));

            if (shop == null)
            {
                return Json(new AjaxResult() { state = "err", msg = "找不到此商品" });
            }

            return Json(new AjaxResult() { state = "ok", data = shop });
        }

        [HttpGet]
        public async Task<ActionResult> Download()
        {
            return File(Server.MapPath("~/Files/库存模板.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        #endregion


        #region

        [HttpGet]
        public async Task<ActionResult> ShopIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ShopList(Guid id)
        {
            ViewData["StockRuleId"] = id;

            var list = await _stockRuleService.GetListShopAsync(id);

            var shopList = Bak365Service.GetShopList();

            foreach (var item in list)
            {
                item.ShopName = shopList[item.ShopNo];
            }

            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> ShopCreate(Guid id)
        {
            ViewData["StockRuleId"] = id;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ShopCreate(StockRuleShopDTO input)
        {
            //判断门店是否存在
            var dto = await _stockRuleService.GetShopAsync(Global.USER_ID, input.ShopNo);

            if (dto != null)
            {
                return Json(new AjaxResult() { state = "err", msg = "该门店已经设置过模板" });
            }

            await _stockRuleService.CreateShopAsync(input);

            return Json(new AjaxResult() { state = "ok" });
        }

        [HttpPost]
        public async Task<ActionResult> GetShop(string shopNo)
        {
            var shop = await Task.FromResult(Bak365Service.GetShopInfo(shopNo));

            if (shop == null)
            {
                return Json(new AjaxResult() { state = "err", msg="找不到此门店" });
            }

            return Json(new AjaxResult() { state = "ok", data = shop });
        }

        [HttpPost]
        public async Task<ActionResult> ShopDelete(Guid id)
        {
            await _stockRuleService.DeleteShopAsync(id);

            return Json(new AjaxResult() { state = "ok" });
        }

        #endregion
    }
}