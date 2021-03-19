using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Common
{
    public class EleConfig
    {
        ////沙箱环境参数
        //public static string APP_KEY = "QUs9DljPyU";                                     //对应Key 表示沙箱环境的应用Key
        //public static string APP_SECRET = "33f26e2546872f27ac2c73f67528ad60c9e6bc55";    //对应Secret 表示沙箱环境的应用Secret
        //public static long STORE_ID = 163095896;                                         //对应沙箱环境店铺ID 
        //public static string STORE_URL = "https://www.ele.me/shop/163095896";            //对应沙箱环境店铺URL
        //public static string STORE_PASS = "";                                            //对应沙箱环境店铺密码
        //public static string REDIRECT_URL = "https://tuangou.bak365.net/New/api/EleCallBack/Authorize"; //对应沙箱环境回调地址URL
        //public static string TOKEN_URL = "https://open-api-sandbox.shop.ele.me/token";
        //public static string API_URL = "https://open-api-sandbox.shop.ele.me/api/v1/";
        //public static string AUTHORIZE_URL = "https://open-api-sandbox.shop.ele.me/authorize";

        //正式环境参数
        public static string APP_KEY = "RNDfkG5r4t";
        public static string APP_SECRET = "38be1e30ed41200964949612f62d03c2bdd16233";
        public static long STORE_ID = -1;
        public static string STORE_URL = "";
        public static string STORE_PASS = "";
        public static string REDIRECT_URL = "https://tuangou.bak365.net/New/api/EleCallBack/Authorize";
        public static string TOKEN_URL = "https://open-api.shop.ele.me/token";
        public static string API_URL = "https://open-api.shop.ele.me/api/v1/";
        public static string AUTHORIZE_URL = "https://open-api.shop.ele.me/authorize";
    }
}
