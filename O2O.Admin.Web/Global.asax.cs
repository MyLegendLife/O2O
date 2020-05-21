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
            //������־���
            log4net.Config.XmlConfigurator.Configure();

            //�Զ����쳣����
            GlobalFilters.Filters.Add(new O2OExceptionFilter());

            //����Բ�ǰ�ǺͿո����� 
            ModelBinders.Binders.Add(typeof(string), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(int), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(long), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(double), new CommonModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new CommonModelBinder());

            //IOC����(Autofac)
            var builder = new ContainerBuilder();
            //�ѵ�ǰ�����е�Controller��ע��
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            //��ȡO2O.Service�еĳ���
            Assembly[] assemblies = new Assembly[] { Assembly.Load("O2O.Service") };
            //��������������µĽӿ�����ķ���ʱ�򣬾ͻ᷵�ض�Ӧ��Service�������ʵ��
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type))  //���ǳ���&&ʵ��IServiceSupport�ӿ�
                .AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            //ע��ϵͳ�����DependencyResolver��������MVC��ܴ���Controller�ȶ����ʱ���ǹ�AudofacҪ����
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
