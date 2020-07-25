using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.Common.Eleme;
using O2O.IService;
using O2O.Service.Eleme;
using System;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Refresh(Guid id)
        {
            var dto = _service.Get(id);

            EleUserApiService service = new EleUserApiService();

            var res = service.RefreshToken(dto.RefreshToken);

            if (res == "") return Json(Tools.ResultErr());

            JObject jo = JObject.Parse(res);

            if (jo["error"] != null) return Json(Tools.ResultErr(jo["error_description"].ToString()));

            dto.AccessToken = jo["access_token"].ToString();
            dto.RefreshToken = jo["refresh_token"].ToString();
            dto.ExpiresDate = DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString()));

            _service.Update(dto);

            return Json(new AjaxResult() { state = "ok" });
        }

        [HttpPost]
        public ActionResult Access(string accountNo, string accountName)
        {
            string url = GetAuthorizeUrl(accountNo, accountName);

            //System.Diagnostics.Process.Start(url);

            return Json(new AjaxResult() { state = "ok", msg=url});
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