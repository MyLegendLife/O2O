using O2O.BackgroundJobs.Tools;
using System.ServiceProcess;

namespace O2O.BackgroundJobs
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //在希望开始调试的地方加入这一行代码
            System.Diagnostics.Debugger.Launch();

            //启动日志框架
            log4net.Config.XmlConfigurator.Configure();

            //windows服务初始化工作
            AutofacUtil.InitAutofac();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
