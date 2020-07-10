using Newtonsoft.Json.Linq;
using O2O.Common;

namespace O2O.Service.Meituan
{
    public class MtOrderApiService : MtBaseApiService
    {
        

        public MtOrderApiService(string userId, string shopNo) : base(userId, shopNo)
        {

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
    }
}