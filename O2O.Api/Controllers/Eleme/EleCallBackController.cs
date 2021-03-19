using log4net;
using Newtonsoft.Json.Linq;
using O2O.Api.App_Code;
using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Service.Eleme;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace O2O.Api.Controllers.Eleme
{
    [RoutePrefix("api/EleCallBack")]
    public class EleCallBackController : ApiController
    {
        private static ILog _log = LogManager.GetLogger("Eleme");
        public IEleAccountService _serviceAccount { get; set; }

        [Route("Authorize")]
        [HttpGet]
        public IHttpActionResult Authorize(string code = "", string state= "")
        {
            try
            {
                var userId = state.Split('@')[0];
                var accountNo = state.Split('@')[1];
                var accountName = state.Split('@')[2];

                var service = new EleUserApiService();

                var res = service.GetToken(code);

                if (res == "") return Json(Tools.ResultErr());

                var jo = JObject.Parse(res);

                if (jo["error"] != null) return Json(Tools.ResultErr(jo["error_description"].ToString()));

                var dto = new Ele_AccountDTO();

                dto.AccountNo = accountNo;
                dto.AccountName = accountName;
                dto.UserId = userId;
                dto.AccessToken = jo["access_token"].ToString();
                dto.TokenType = jo["token_type"].ToString();
                dto.RefreshToken = jo["refresh_token"].ToString();
                dto.Scope = jo["scope"]?.ToString();
                dto.ExpiresDate = DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString()));

                _serviceAccount.Add(dto);

                return Json(new { message = "ok" });
            }
            catch (Exception e)
            {
                _log.DebugFormat("【信息记录】用户:{0}  类型:Authorize  信息{1}", state, e.Message);
                _log.DebugFormat("【信息记录】用户:{0}  类型:Authorize  信息{1}", state, e.GetOriginalException().Message);
                return Json(new { message = "ok" });
            }
        }

        [Route("PushMessage")]
        [HttpPost]
        public IHttpActionResult PushMessage()
        {
            var context = HttpContext.Current;
            var task = Task.Factory.StartNew((con) =>
            {
                var res = HttpCommon.PostReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】类型:Push  信息{0}", res);
                    var service = new EleCallBackService();
                    service.HandlePushMessage(res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】类型:Push 信息：{0} 错误：{1}", res, e.Message);
                    _log.DebugFormat("【系统错误】类型:Push 信息：{0} 错误：{1}", res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { message = "ok" });
        }

        [Route("PushMessage")]
        [HttpGet]
        public IHttpActionResult PushMessage(string userId = "")
        {
            return Json(new { message = "ok" });
        }
    }
}
