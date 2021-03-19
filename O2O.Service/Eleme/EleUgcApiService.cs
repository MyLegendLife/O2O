using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;

namespace O2O.Service.Eleme
{
    public class EleUgcApiService : EleBaseApiService
    {
        /// <summary>
        /// 统计评价信息数量
        /// </summary>
        /// <returns></returns>
        public EleResult CountORateResult(string token, object rateQuery)
        {
            var model = new { rateQuery };
            var sign = GetSign(token, model, "eleme.ugc.countORateResult");
            var content = MakeNopEntity(sign, model);
            var res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        ///  获取指定店铺的评论
        /// </summary>
        /// <returns></returns>
        public EleResult GetOrderRatesByShopId(string token, long shopId, string startTime, string endTime, int offset, int pageSize)
        {
            var model = new { shopId, startTime, endTime, offset, pageSize };
            var sign = GetSign(token, model, "eleme.ugc.getOrderRatesByShopId");
            var content = MakeNopEntity(sign, model);
            var res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        ///  查询评价信息
        /// </summary>
        /// <returns></returns>
        public EleResult GetORateResult(string token, long shopId, string startTime, string endTime, int offset, int pageSize)
        {
            var model = new { shopId, startTime, endTime, offset, pageSize };
            var sign = GetSign(token, model, "eleme.ugc.getORateResult");
            var content = MakeNopEntity(sign, model);
            var res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }
    }
}