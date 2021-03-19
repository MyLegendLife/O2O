using System.Reflection;
using Autofac;
using Hangfire;
using O2O.BackgroundJobs.Jobs;

namespace O2O.BackgroundJobs.Tools
{
    public class AutofacUtil
    {/// <summary>
        /// Autofac容器对象
        /// </summary>
        private static Autofac.IContainer _container;

        /// <summary>
        /// 初始化autofac
        /// </summary>
        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            //注入仓储类
            builder.RegisterAssemblyTypes(Assembly.Load("O2O.Service"))
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            //注册当前程序集的所有类成员
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces().AsSelf();

            _container = builder.Build();

            //Hangfire&Autofac结合
            GlobalConfiguration.Configuration.UseAutofacActivator(_container);
        }

        /// <summary>
        /// 从Autofac容器获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
