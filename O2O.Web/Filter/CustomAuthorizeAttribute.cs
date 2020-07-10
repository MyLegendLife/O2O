using System.Web;
using System.Web.Mvc;

namespace O2O.Web.App_Start
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.RawUrl.Contains("Login"))
            {
                if (HttpContext.Current.Session["UserId"] is null)
                {
                    httpContext.Response.Redirect("/Login/Index");
                    return true;
                }
            }
            return true;
        }
    }
}