using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(O2O.Web.Startup))]

namespace O2O.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("connStr");
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //hangfire 身份验证过滤器
                Authorization = new[] { new CustomerHangfireDashboardFilter() },
            });
        }
    }

    public class CustomerHangfireDashboardFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {

            return true;
        }
    }
}
