using Newtonsoft.Json.Linq;
using O2O.Api.App_Code;
using O2O.Service.Meituan;
using System.Web.Http;

namespace O2O.Api.Controllers.Meituan
{
    [RoutePrefix("api/Coupon")]
    public class CouponController : ApiController
    {
        /// <returns></returns>
        [Route("Query")]
        [HttpPost]
        public IHttpActionResult Query(string userId, string shopNo, string codeNo)
        {
            var service = new MtCouponService(userId, shopNo);

            //var res = service.Query(codeNo);
            var res = service.Prepare(codeNo);

            return Json(res);
        }


            /// <summary>
            /// 验券 只要有失败就中断，并返回失败
            /// </summary>
            /// <param name="userNo"></param>
            /// <param name="shopNo"></param>
            /// <param name="data">
            /// {
            ///       "OrderID":"000001",
            ///      "TtlPrice":50.5,
            ///       "Codes":"12456123123123,12456123123123,12456123123123",
            ///       "OperNo":"收银员编号",
            ///       "OperName":"收银员"
            /// }
            /// </param>
            /// <returns></returns>
            [Route("Consume")]
        [HttpPost]
        public IHttpActionResult Consume(string userId, string shopNo, [FromBody] JObject data)
        {
            var  service = new MtCouponService(userId, shopNo);

            var res = service.Consume(data);

            return Json(res);
        }

        /// <summary>
        /// 撤销验券 只要不是系统报错，都返回成功，如果有撤销失败，返回 成功+失败原因
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="shopNo"></param>
        /// <param name="data">
        /// {
        ///       "Codes":"12456123123123,12456123123123,12456123123123",
        ///       "OperNo":"收银员编号",
        ///       "OperName":"收银员"
        /// }
        /// </param>
        /// <returns></returns>
        [Route("Cancel")]
        [HttpPost]
        public IHttpActionResult Cancel(string userId, string shopNo, [FromBody] JObject data)
        {
            var service = new MtCouponService(userId, shopNo);

            var res = service.Cancel(data);

            return Json(res);
        }
    }
}
