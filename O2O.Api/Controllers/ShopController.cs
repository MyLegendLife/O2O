using log4net;
using O2O.Common;
using O2O.Service;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace O2O.Api.Controllers
{
    [RoutePrefix("api/Shop")]
    public class ShopController : ApiController
    {
        private static ILog _log = LogManager.GetLogger("Shop");

        [Route("GetConfirmState")]
        [HttpPost]
        public async Task<IHttpActionResult> GetConfirmState(string userId, string shopNo)
        {
            try
            {
                var service = new ShopConfigService();

                var dto = await service.GetAsync(userId, shopNo);

                return Json(Tools.ResultOk(dto));
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:GetConfirmState  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:GetConfirmState  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("SetConfirmState")]
        [HttpPost]
        public async Task<IHttpActionResult> SetConfirmState(string userId, string shopNo, int mtState, int eleState)
        {
            try
            {
                var service = new ShopConfigService();

                await service.SetAsync(userId, shopNo, mtState, eleState);

                return Json(Tools.ResultOk());
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:SetConfirmState  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:SetConfirmState  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }
    }
}