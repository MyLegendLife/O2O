using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Common
{
    public class Tools
    {
        public static Result ResultMt(string str)
        {
            var result = new Result();

            JObject json = JObject.Parse(str);
            if (json["error"] != null)
            {
                result.State = "ERR";
                result.Msg = json["error"]["msg"].ToString();
            }
            else
            {
                result.State = "OK";
                result.Msg = "";
                result.Data = json["data"];
            }

            return result;
        }

        public static Result ResultEle(string str)
        {
            var result = new Result();

            return result;
        }

        public static Result ResultOk()
        {
            return new Result() { State = "OK", Msg = "" };
        }

        public static Result ResultOk(object data)
        {
            return new Result() { State = "OK", Msg = "", Data = data };
        }

        public static Result ResultErr(string msg)
        {
            return new Result() { State = "ERR", Msg = msg };
        }

        public static Result ResultErr()
        {
            return new Result() { State = "ERR", Msg="网络错误" };
        }

        public static Result ToResult(EleResult eleRes)
        {
            if (eleRes.error != null)
            {
                return new Result() { State = "ERR", Msg = eleRes.error.message };
            }
            else
            {
                return new Result() { State = "OK", Msg = "", Data = eleRes.result };
            }
        }
    }
}