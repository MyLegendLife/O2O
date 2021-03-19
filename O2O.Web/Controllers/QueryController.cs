using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using O2O.IService;
using O2O.Service;

namespace O2O.Web.Controllers
{
	public class QueryController : Controller
	{
		public IOrderService _orderService { get; set; }

		public ActionResult Index()
		{
			return base.View();
		}

		public ActionResult List()
		{
			
			return View();
		}
	}
}
