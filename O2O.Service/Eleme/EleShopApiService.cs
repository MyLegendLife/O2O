using Newtonsoft.Json;
using O2O.Common;

namespace O2O.Service.Eleme
{
    public class EleShopApiService : EleBaseApiService
    {
        /// <summary>
        /// 更新店信息
        /// </summary>
        /// <param name="_shopId"></param>
        /// <param name="pShopInfo"></param>
        /// <returns></returns>
        public EleResult UpdateShop(string token, long _shopId, object obj)
        {
            var model = new { shopId = _shopId, properties = obj };
            SignParams sign = GetSign(token, model, "eleme.shop.updateShop");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }
    }
}