using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.Common.Eleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace O2O.Service.Eleme
{
    public class EleBaseApiService
    {
        public EleBaseApiService()
        {

        }

        /// <summary>
        /// 获取饿了么访问令牌
        /// </summary>
        /// <returns></returns>
        public string GetToken(string code)
        {
            //1.拼接key && Secret
            string merger = EleConfig.APP_KEY + ":" + EleConfig.APP_SECRET;
            //2.Base64编码
            string strBase64 = ToolsCommon.ToBase64(merger);
            //构建header
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Basic " + strBase64);

            string content = "grant_type=authorization_code&code=" + code + "&redirect_uri=" + EleConfig.REDIRECT_URL + "&client_id=" + EleConfig.APP_KEY;
            string res = HttpCommon.Post(EleConfig.TOKEN_URL, "application/json;charset=utf-8", headers, content);

            return res;
        }

        /// <summary>
        /// Token过期时，刷新获取新的Token
        /// </summary>
        /// <param name="refreshCode"></param>
        /// <returns></returns>
        public JObject RefreshToken(string refreshCode)
        {
            //1.拼接key && Secret
            string merger = EleConfig.APP_KEY + ":" + EleConfig.APP_SECRET;
            //2.Base64编码
            string strBase64 = ToolsCommon.ToBase64(merger);
            //构建header
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Basic " + strBase64);

            string content = "grant_type=refresh_token&refresh_token=" + refreshCode;
            string ret = HttpCommon.Post(EleConfig.TOKEN_URL, "application/json;charset=utf-8", headers, content);
            JObject json = JObject.Parse(ret);
            return json;
        }

        /// <summary>
        /// 取Sign
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected SignParams GetSign(string token, object obj, string action)
        {
            long timestamp = ToolsCommon.GetTimestamp();
            JObject jObj = JObject.FromObject(obj);
            jObj.Add("app_key", EleConfig.APP_KEY);
            jObj.Add("timestamp", timestamp);
            string sortJson = JsonSort.SortJson(jObj, null);
            ////替换掉属性排序关键字
            //Regex reg = new Regex("[a-z]__");
            //sortJson = reg.Replace(sortJson, "");
            jObj = JObject.Parse(sortJson);
            List<JProperty> listProperty = jObj.Properties().ToList();
            StringBuilder buff = new StringBuilder();
            foreach (JProperty attr in listProperty)
            {
                string pKey = attr.Name;
                string pValue = attr.Value.ToString(Newtonsoft.Json.Formatting.None);
                if (attr.Value.Type == JTokenType.String)
                {
                    pValue = "\"" + attr.Value.ToString() + "\"";
                }
                else if (attr.Value.Type == JTokenType.Float)
                {
                    pValue = float.Parse(pValue).ToString();
                }
                else if (attr.Value.Type == JTokenType.Object)
                {

                }
                buff.Append(pKey + "=" + pValue);
            }

            //完整拼接字符串
            string fullParams = action + token + buff.ToString() + EleConfig.APP_SECRET;
            string sign = ToolsCommon.GetMD5(fullParams).ToUpper();
            SignParams pSignParams = new SignParams();
            pSignParams.sign = sign;
            pSignParams.timestamp = timestamp;
            pSignParams.token = token;
            pSignParams.action = action;
            return pSignParams;
        }

        /// <summary>
        /// 构建NOP对象
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timestamp"></param>
        /// <param name="params_repl_"></param>
        /// <param name="sign"></param>
        protected string MakeNopEntity(string _action, string token, long timestamp, object params_repl_, string sign)
        {
            NopBody nopBody = new NopBody();
            nopBody.action = _action;
            nopBody.metas = new { app_key = EleConfig.APP_KEY, timestamp = timestamp };
            nopBody.token = token;
            nopBody.params_repl_ = params_repl_;
            nopBody.signature = sign;
            string retStr = JObject.FromObject(nopBody).ToString(Formatting.None).Replace("_repl_", "");
            //替换掉属性排序关键字
            Regex reg = new Regex("[a-z]__");
            retStr = reg.Replace(retStr, "");
            return retStr;
        }

        /// <summary>
        /// 构建NOP对象
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="params_repl_"></param>
        /// <returns></returns>
        protected string MakeNopEntity(SignParams sign, object params_repl_)
        {
            return MakeNopEntity(sign.action, sign.token, sign.timestamp, params_repl_, sign.sign);
        }
    }

    class NopBody
    {
        public string nop { set; get; }

        public string id { set; get; }

        public string action { set; get; }

        public string token { set; get; }

        public object metas { set; get; }

        public object params_repl_ { set; get; }

        public string signature { set; get; }

        public NopBody()
        {
            id = Guid.NewGuid().ToString();
            nop = "1.0.0";
        }
    }

    public class SignParams
    {
        public string sign { set; get; }
        public string action { set; get; }
        public long timestamp { set; get; }
        public string token { set; get; }
    }
}