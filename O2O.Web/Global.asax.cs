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
            //������־���
            log4net.Config.XmlConfigurator.Configure();

            #region IOC����(Autofac)

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();//�ѵ�ǰ�����е� Controller ��ע��
            //��ȡ����������ĳ���
            Assembly[] assemblies = new Assembly[] { Assembly.Load("O2O.Service") };
            //��������������µĽӿ�����ķ���ʱ�򡣾ͻ᷵�ض�Ӧ��Services�������ʵ��
            builder.RegisterAssemblyTypes(assemblies)
            .Where(type => !type.IsAbstract
                    && typeof(IServiceSupport).IsAssignableFrom(type))
                    .AsImplementedInterfaces().PropertiesAutowired();
            //Assign����ֵ
            //type1.IsAssignableFrom(type2)   type2�Ƿ�ʵ����type1�ӿ�/type2�Ƿ�̳���type1
            //typeof(IServiceSupport).IsAssignableFrom(type)ֻע��ʵ����IServiceSupport����
            //���������޹ص���ע�ᵽAutoFac��

            var container = builder.Build();
            //ע��ϵͳ�����DependencyResolver��������MVC��ܴ���Controller�ȶ����ʱ���ǹ�AutofacҪ����
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            #endregion

            //�Զ����쳣����
            GlobalFilters.Filters.Add(new O2OExceptionFilter());
            //Ȩ�޿��ƣ�Ŀǰֻ������¼��֤
            GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());

            //����Բ�ǰ�ǺͿո����� 
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
