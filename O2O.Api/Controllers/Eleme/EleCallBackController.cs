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
                string userId = state.Split('@')[0];
                string accountNo = state.Split('@')[1];
                string accountName = state.Split('@')[2];

                EleUserApiService service = new EleUserApiService();

                var res = service.GetToken(code);

                if (res == "") return Json(Tools.ResultErr());

                JObject jo = JObject.Parse(res);

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
                return Json(new { message = "ok" });
            }
        }

        [Route("PushMessage")]
        [HttpPost]
        public IHttpActionResult PushMessage()
        {
            var context = HttpContext.Current;
            Task task = Task.Factory.StartNew((con) =>
            {
                try
                {
                    EleCallBackService service = new EleCallBackService();
                    string res = HttpCommon.PostReceive(con);

                    _log.DebugFormat("【信息记录】类型:Push  信息{0}", res);

                    service.HandlePushMessage(res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【错误】用户:{0}  类型:Push  信息{1}", "", e.Message);
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
