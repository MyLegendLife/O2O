using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using O2O.Api.App_Code;
using O2O.Api.App_Start;
using O2O.Common;
using O2O.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace O2O.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //NoticeCommon.Notice("0003","","");

            //启动日志框架
            log4net.Config.XmlConfigurator.Configure();

            //AutoFacConfig.Register();

            #region IOC配置(Autofac)

            var builder = new ContainerBuilder();

            //builder.RegisterType<BaseApiService>();
            //Assembly assembly = Assembly.Load("O2O.Api");
            //builder.RegisterType(assembly.GetType("O2O.Api.App_Code.Meituan.BaseApiService"));

            //把当前程序集中的Controller都注册
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            //builder.RegisterType<ElemeApiService>().InstancePerLifetimeScope();

            //获取O2O.Service中的程序集
            Assembly[] assemblies = new Assembly[] { Assembly.Load("O2O.Service") };

            //当请求这个程序集下的接口里面的方法时候，就会返回对应的Service类里面的实现
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type))  //不是抽象&&实现IServiceSupport接口
                .AsImplementedInterfaces().PropertiesAutowired();

            var container = builder.Build();
            //注册api容器需要使用HttpConfiguration对象
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            #endregion

            //自定义异常处理
            GlobalFilters.Filters.Add(new O2OExceptionFilter());

            //处理圆角半角和空格问题 
            ModelBinders.Binders.Add(typeof(string), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(int), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(long), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(double), new ModelBinderCommon());
            ModelBinders.Binders.Add(typeof(decimal), new ModelBinderCommon());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
