using O2O.Common;
using O2O.Common.Eleme;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Service;
using O2O.Web.App_Start;
using O2O.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace O2O.Web.Areas.Eleme.Controllers
{
    public class AccountController : Controller
    {
        public string _url;
        public IEleAccountService _service { get; set; }

        [HttpGet]
        public ActionResult List()
        {
            var list = _service.GetAccounts(Global.USER_ID);

            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            bool b = _service.Delete(id);

            return Json(new AjaxResult() { state = b ? "ok" : "no" });
        }

        public ActionResult Access(string accountNo, string accountName)
        {
            string url = GetAuthorizeUrl(accountNo, accountName);

            System.Diagnostics.Process.Start(url);

            return Json("");
        }

        public string GetAuthorizeUrl(string accountNo, string accountName)
        {
            string userId = Global.USER_ID.ToString();   //将客户标识传到饿了么，饿了么会回传，以免丢失

            string url = string.Format(EleConfig.AUTHORIZE_URL + "?" +
                "response_type=code" +
                "&client_id={0}" +
                "&redirect_uri={1}" +
                "&scope=all" +
                "&state={2}",
            EleConfig.APP_KEY, HttpUtility.UrlDecode(EleConfig.REDIRECT_URL), userId + "@" + accountNo + "@" + accountName);
            
            return url;
        }
    }
}