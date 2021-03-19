using log4net;
using O2O.Api.App_Code;
using O2O.Common;
using O2O.IService;
using O2O.Service;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace O2O.Api.Controllers.Meituan
{
    [RoutePrefix("api/MtCallBack")]
    public class MtCallBackController : ApiController
    {
        private static ILog _log = LogManager.GetLogger("Meituan");
        MtCallBackService _service { get; set; }        

        public MtCallBackController()
        {
            _service = new MtCallBackService();
        }

        #region 订单推送

        [Route("OrderPayed")]
        [HttpPost]
        public IHttpActionResult OrderPayed(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.PostReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】用户:{0}  类型:OrderPayed 信息{1}", userId, res);
                    _service.OrderPayed(userId, res);                    
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】用户:{0}  类型:OrderPayed 信息:{1} 错误:{2}", userId, res, e.Message);
                    _log.DebugFormat("【系统错误】用户:{0}  类型:OrderPayed 信息:{1} 错误:{2}", userId, res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { data = "ok" });
        }

        [Route("OrderConfirmed")]
        [HttpPost]
        public IHttpActionResult OrderConfirmed(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.PostReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】用户:{0}  类型:OrderConfirmed 信息{1}", userId, res);
                    _service.OrderConfirmed(userId, res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】用户:{0}  类型:OrderConfirmed 信息:{1} 错误:{2}", userId, res, e.Message);
                    _log.DebugFormat("【系统错误】用户:{0}  类型:OrderConfirmed 信息:{1} 错误:{2}", userId, res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { data = "ok" });
        }

        [Route("OrderFinished")]
        [HttpPost]
        public IHttpActionResult OrderFinished(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.PostReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】用户:{0}  类型:OrderFinished 信息{1}", userId, res);
                    _service.HandlePush("OrderFinished",userId, res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】用户:{0}  类型:OrderFinished 信息:{1} 错误:{2}", userId, res, e.Message);
                    _log.DebugFormat("【系统错误】用户:{0}  类型:OrderFinished 信息:{1} 错误:{2}", userId, res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { data = "ok" });
        }

        [Route("OrderCanceled")]
        [HttpGet]
        public IHttpActionResult OrderCanceled(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.GetReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】类型:OrderCanceled 信息{0}", res);
                    _service.HandlePush("OrderCanceled", userId, res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】类型:OrderCanceled 信息:{0} 错误:{1}", res, e.Message);
                    _log.DebugFormat("【系统错误】类型:OrderCanceled 信息:{0} 错误:{1}", res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { data = "ok" });
        }

        [Route("OrderRefunded")]
        [HttpGet]
        public IHttpActionResult OrderRefunded(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.GetReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】类型:OrderRefunded 信息{0}", res);
                    _service.OrderRefund(res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】类型:OrderRefunded 信息:{0} 错误:{1}", res, e.Message);
                    _log.DebugFormat("【系统错误】类型:OrderRefunded 信息:{0} 错误:{1}", res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { data = "ok" });
        }

        [Route("OrderRefundedPart")]
        [HttpGet]
        public IHttpActionResult OrderRefundedPart(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.GetReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】类型:OrderRefundedPart 信息{0}", res);
                    _service.OrderRefundPart(res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】类型:OrderRefundedPart 信息:{0} 错误:{1}", res, e.Message);
                    _log.DebugFormat("【系统错误】类型:OrderRefundedPart 信息:{0} 错误:{1}", res, e.GetOriginalException().Message);
                }
            }, context);

            return Json(new { data = "ok" });
        }

        [Route("OrderUrge")]
        [HttpPost]
        public IHttpActionResult OrderUrge(string userId)
        {
            //var context = HttpContext.Current;

            //Task task1 = Task.Factory.StartNew((con) =>
            //{
            //    string res = HttpCommon.PostReceive(con);
            //    try
            //    {
            //        _log.DebugFormat("【信息记录】用户:{0}  类型:OrderUrge 信息{1}", userId, res);
            //        _service.Handle("OrderUrge", res, userId);
            //    }
            //    catch (Exception e)
            //    {
            //        _log.DebugFormat("【系统错误】用户:{0}  类型:OrderUrge 信息:{1} 错误:{2}", userId, res, e.GetOriginalException().Message);
            //    }
            //}, context);
            return Json(new { data = "ok" });
        }

        [Route("OrderDeliveringStatus")]
        [HttpPost]
        public IHttpActionResult OrderDeliveringStatus(string userId)
        {
            var context = HttpContext.Current;

            Task task = Task.Factory.StartNew((con) =>
            {
                string res = HttpCommon.PostReceive(con);
                try
                {
                    _log.DebugFormat("【信息记录】类型:OrderDeliveringStatus 信息{0}", res);
                    _service.HandlePush("OrderDeliveringStatus", "",res);
                }
                catch (Exception e)
                {
                    _log.DebugFormat("【系统错误】用户:{0} 类型:OrderDeliveringStatus 信息:{1} 错误:{2}", "",res, e.Message);
                    _log.DebugFormat("【系统错误】用户:{0} 类型:OrderDeliveringStatus 信息:{1} 错误:{2}", "",res, e.GetOriginalException().Message);
                }
            }, context);
            return Json(new { data = "ok" });
        }

        [Route("OrderPrivacy")]
        [HttpPost]
        public IHttpActionResult OrderPrivacy(string userId)
        {
            return Json(new { data = "ok" });
        }

        #endregion
    }
}
