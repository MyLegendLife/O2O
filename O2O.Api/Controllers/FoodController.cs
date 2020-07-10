using log4net;
using Newtonsoft.Json.Linq;
using O2O.Api.App_Code;
using O2O.Common;
using O2O.Service.Meituan;
using System;
using System.Web.Http;

namespace O2O.Api.Controllers
{
    [RoutePrefix("api/Food")]
    public class FoodController : ApiController
    {
        private static ILog _log = LogManager.GetLogger("Food");

        [Route("GetFood")]
        [HttpPost]
        public IHttpActionResult GetFood(string userId, string shopNo, int takeType)
        {
            try
            {
                if (takeType == 0)
                {
                    MtFoodApiService service = new MtFoodApiService(userId, shopNo);
                    var res = service.List();

                    return Json(res);
                }
                else
                {
                    EleFoodService service = new EleFoodService();
                    var res = service.List(userId, shopNo);

                    return Json(res);
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【错误】类型:GetFood  信息{0}", e.Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("UpdateStock")]
        [HttpPost]
        public IHttpActionResult UpdateStock(string userId, string shopNo, int takeType,[FromBody]JArray data)
        {
            try
            {
                if (takeType == 0)
                {
                    MtFoodApiService service = new MtFoodApiService(userId, shopNo);
                    var res = service.SkuStock(data);

                    return Json(res);
                }
                else
                {
                    EleFoodService service = new EleFoodService();
                    var res = service.SkuStock(userId, shopNo, data);

                    return Json(res);
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【错误】UpdateStock  信息{0}", e.Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }
    }
}