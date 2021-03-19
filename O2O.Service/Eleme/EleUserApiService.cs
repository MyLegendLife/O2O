using Newtonsoft.Json;
using O2O.Common;

namespace O2O.Service.Eleme
{
    public class EleUserApiService : EleBaseApiService
    {
        /// <summary>
        /// 取商户信息
        /// </summary>
        /// <returns></returns>
        public EleResult GetUser(string token)
        {
            object obj = new object();
            SignParams sign = GetSign(token, obj, "eleme.user.getUser");
            string content = MakeNopEntity(sign, obj);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }
    }
}