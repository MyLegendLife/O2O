using System;
using System.Web;

namespace O2O.Common
{
    public class Global
    {
        public static string USER_ID => HttpContext.Current.Session["UserId"]?.ToString();

        public static string CON_STR
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["CON_STR"]?.ToString();
                }
                catch
                {
                    return "";
                }
            }
            set => HttpContext.Current.Session["CON_STR"] = value;
        }
    }
}
