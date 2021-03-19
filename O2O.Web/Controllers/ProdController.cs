using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class ProdController : Controller
	{
		public IOrderService _orderService { get; set; }

		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult List()
		{
			var orderProdList = _orderService.GetOrderProdList(Global.USER_ID, "");
			return View("List", orderProdList);
		}

		[HttpPost] 
		public PartialViewResult SearchList(string text)
		{
			var orderProdList = _orderService.GetOrderProdList(Global.USER_ID, text);
			return PartialView(orderProdList);
		}

		[HttpGet]
		public ActionResult Edit(string prodNo, string prodName)
		{
			ViewBag.ProdNo = prodNo;
			ViewBag.ProdName = prodName;
			
			return View();
		}

		[HttpPost]
		public ActionResult Edit(string prodNo, string prodName, string prodNoNew)
		{
			_orderService.UpdateProdNoMapInfo(Global.USER_ID, prodNo, prodName, prodNoNew);
			return Json(new AjaxResult
			{
				state = "ok",
				msg = ""
			});
		}

		[HttpPost]
		public ActionResult Delete(string prodNo, string prodName)
		{
			_orderService.DeleteProdNoMapInfo(Global.USER_ID, prodNo, prodName);
			return Json(new AjaxResult
			{
				state = "ok",
				msg = ""
			});
		}
	}
}