using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace O2O.Api.App_Start
{
    public class AutoFacConfig
    {
        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序及中的类型对象的创建
        /// 负责创建WebApi控制器类的对象（调用控制器中的有参构造函数）,接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register()

        {
            // 实例化一个autofac的创建容器
            var builder = new ContainerBuilder();

            SetupResolveRules(builder);

            //注册所有的Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // 创建一个autofac的容器
            var container = builder.Build();

            // 控制器对象由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

        /// <summary>
        /// 设置配置规则
        /// </summary>
        /// <param name="builder"></param>
        private static void SetupResolveRules(ContainerBuilder builder)
        {
            // 告诉autofac框架，将来要创建的控制器类存放在哪个程序集
            Assembly controllerAssmbly = Assembly.Load("Common_Management.Web");

            builder.RegisterControllers(controllerAssmbly);
            //// 如果需要直接调用仓储层
            //// 告诉autofac框架注册数据仓储层所在程序集中的所有类的对象实例
            //Assembly RepositoryAssembly = Assembly.Load("CommonManagement.Repository");
            //// 创建仓储层中的所有类的instance以此类的实现接口存储
            //builder.RegisterTypes(RepositoryAssembly.GetTypes()).Where(a => a.Name.Contains("Repository")).AsImplementedInterfaces();

            // 告诉autofac框架注册数据业务层(应用层)所在程序集中的所有类的对象实例
            Assembly ServiceAssembly = Assembly.Load("Common_Management.Application");

            // 创建应用层中的所有类的instance以此类的实现接口存储
            builder.RegisterTypes(ServiceAssembly.GetTypes()).Where(a => a.Name.Contains("Application")).AsImplementedInterfaces();
        }
    }
}