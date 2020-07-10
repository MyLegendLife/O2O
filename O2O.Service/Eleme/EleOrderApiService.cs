using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.Common.Eleme;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace O2O.Service.Eleme
{
    public class EleOrderApiService : EleBaseApiService
    {
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="businNo"></param>
        /// <param name="shopNo"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public EleResult GetAllOrders(string token, long shopId, int pageNo, int pageSize, string date)
        {
            var model = new { shopId = shopId, pageNo = pageNo, pageSize = pageSize, date = date };
            SignParams sign = GetSign(token, model, "eleme.order.getAllOrders");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        // <summary>
        /// 获取单个订单
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public EleResult GetOrder(string token, string orderId)
        {
            var model = new { orderId = orderId };
            SignParams sign = GetSign(token, model, "eleme.order.getOrder");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public EleResult ConfirmOrderLite(string token, string orderId)
        {
            var model = new { orderId = orderId };
            SignParams sign = GetSign(token, model, "eleme.order.confirmOrderLite");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="_orderId"></param>
        /// <param name="_type"></param>
        /// <param name="_remark"></param>
        /// <returns></returns>
        public EleResult CancelOrderLite(string token, string orderId, JObject data)
        {
            var model = new { orderId = orderId, type = data["Code"], remark = data["Reason"] };
            SignParams sign = GetSign(token, model, "eleme.order.cancelOrderLite");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 取消单_同意退单
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public EleResult AgreeRefundLite(string token, string orderId)
        {
            var model = new { orderId = orderId };
            SignParams sign = GetSign(token, model, "eleme.order.agreeRefundLite");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 取消单_不同意退单
        /// </summary>
        /// <param name="_orderId"></param>
        /// <param name="_reason"></param>
        /// <returns></returns>
        public EleResult DisagreeRefundLite(string token, string orderId, JObject data)
        {
            var model = new { orderId = orderId, reason = data["Reason"] };
            SignParams sign = GetSign(token, model, "eleme.order.disagreeRefundLite");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 订单确认送达
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public EleResult ReceivedOrderLite(string token, string _orderId)
        {
            var model = new { orderId = _orderId };
            SignParams sign = GetSign(token, model, "eleme.order.receivedOrderLite");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }
    }
}