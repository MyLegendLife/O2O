using Autofac;
using Autofac.Integration.Mvc;
using O2O.Common;
using O2O.IService;
using O2O.Web.App_Start;
using O2O.Web.Filter;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace O2O.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //启动日志框架
            log4net.Config.XmlConfigurator.Configure();

            #region IOC配置(Autofac)

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();//把当前程序集中的 Controller 都注册
            //获取所有相关类库的程序集
            Assembly[] assemblies = new Assembly[] { Assembly.Load("O2O.Service") };
            //当请求这个程序集下的接口里面的方法时候。就会返回对应的Services类里面的实现
            builder.RegisterAssemblyTypes(assemblies)
            .Where(type => !type.IsAbstract
                    && typeof(IServiceSupport).IsAssignableFrom(type))
                    .AsImplementedInterfaces().PropertiesAutowired();
            //Assign：赋值
            //type1.IsAssignableFrom(type2)   type2是否实现了type1接口/type2是否继承自type1
            //typeof(IServiceSupport).IsAssignableFrom(type)只注册实现了IServiceSupport的类
            //避免其他无关的类注册到AutoFac中

            var container = builder.Build();
            //注册系统级别的DependencyResolver，这样当MVC框架创建Controller等对象的时候都是管Autofac要对象。
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            #endregion

            //自定义异常处理
            GlobalFilters.Filters.Add(new O2OExceptionFilter());
            //权限控制，目前只用作登录验证
            GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());

            //处理圆角半角和空格问题 
            ModelBinders.Binders.Add(typeof(string), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(int), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(long), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(double), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(decimal), new ModelBinderCommon());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
