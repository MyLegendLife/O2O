using Autofac;
using Autofac.Integration.Mvc;
using O2O.Admin.Web.App_Start;
using O2O.Common;
using O2O.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace O2O.Admin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //启动日志框架
            log4net.Config.XmlConfigurator.Configure();

            //自定义异常处理
            GlobalFilters.Filters.Add(new O2OExceptionFilter());

            //处理圆角半角和空格问题 
            ModelBinders.Binders.Add(typeof(string), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(int), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(long), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(double), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new CommonModelBinder());

            //IOC配置(Autofac)
            var builder = new ContainerBuilder();
            //把当前程序集中的Controller都注册
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            //获取O2O.Service中的程序集
            Assembly[] assemblies = new Assembly[] { Assembly.Load("O2O.Service") };
            //当请求这个程序集下的接口里面的方法时候，就会返回对应的Service类里面的实现
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type))  //不是抽象&&实现IServiceSupport接口
                .AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            //注册系统级别的DependencyResolver，这样当MVC框架创建Controller等对象的时候都是管Audofac要对象
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
