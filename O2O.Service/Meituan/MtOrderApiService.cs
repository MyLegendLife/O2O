using Newtonsoft.Json.Linq;
using O2O.Common;
using System;

namespace O2O.Service.Meituan
{
    public class MtOrderApiService : MtBaseApiService
    {
        

        public MtOrderApiService(string userId, string shopNo = "") : base(userId, shopNo)
        {

        }

        public Result GetOrderDetail(string orderId)
        {
            var data = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                order_id = orderId
            };
            return Tools.ResultMt(HttpCommon.Get(this.GetUrl("https://waimaiopen.meituan.com/api/v1/order/getOrderDetail", data)));
        }

        public Result Confirm(string orderID)
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                order_id = orderID,
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/confirm", model);

            return Tools.ResultMt(HttpCommon.Get(url));
        }

        /// <summary>
        /// ªÒ»°≈‰ÀÕ∂©µ•◊¥Ã¨
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Result GetLogisticsStatus(string orderID)
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                order_id = orderID,
                is_mt_logistics = 1
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/logistics/status", model);

            return Tools.ResultMt(HttpCommon.Get(url));
        }

        public Result Cancel(string orderID, JObject data)
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                order_id = orderID,
                reason = data["Reason"],
                reason_code = data["Code"]
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/cancel", model);

            return Tools.ResultMt(HttpCommon.Get(url));
        }

        public Result RefundAgree(string orderID, JObject data)
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                order_id = orderID,
                reason = data["Reason"].ToString(),
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/refund/agree", model);

            return Tools.ResultMt(HttpCommon.Get(url));
        }

        public Result RefundReject(string orderID, JObject data)
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                order_id = orderID,
                reason = data["Reason"]
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/refund/reject", model);

            return Tools.ResultMt(HttpCommon.Get(url));
        }

        public Result BatchPullPhoneNumber()
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                app_poi_code = _shopNo,
                offset = 0,
                limit = 999
            };

            var modelBody = new 
            {
                app_poi_code= _shopNo,
                offset = 0,
                limit =999
          };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/batchPullPhoneNumber", model);

            return Tools.ResultMt(HttpCommon.Post(url, modelBody));
        }

        public Result GetRiderInfoPhoneNumber()
        {
            var model = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = _waimaiAppId,
                app_poi_code = _shopNo,
                offset = 0,
                limit = 999
            };

            var modelBody = new
            {
                app_poi_code = _shopNo,
                offset = 0,
                limit = 999
            };

            string url = GetUrl("https://waimaiopen.meituan.com/api/v1/order/getRiderInfoPhoneNumber", model);

            return Tools.ResultMt(HttpCommon.Post(url, modelBody));
        }

        public Result GetOrderIdByDaySeq(DateTime datetime, int daySeq)
        {
            var data = new
            {
                timestamp = ToolsCommon.GetTimestamp(),
                app_id = this._waimaiAppId,
                app_poi_code = this._shopNo,
                date_time = datetime.ToString("yyyyMMdd"),
                day_seq = daySeq
            };
            return Tools.ResultMt(HttpCommon.Get(this.GetUrl("https://waimaiopen.meituan.com/api/v1/order/getOrderIdByDaySeq", (object)data)));
        }
    }
}