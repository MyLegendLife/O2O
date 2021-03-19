using Hangfire;
using Hangfire.SqlServer;
using O2O.BackgroundJobs.Jobs;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace O2O.BackgroundJobs
{
    public partial class Service1 : ServiceBase
    {
        private BackgroundJobServer _server;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var upDownCron = ConfigurationManager.AppSettings["UpDownCron"];
            var syncStockCron = ConfigurationManager.AppSettings["SyncStockCron"];
            var authClock = int.Parse(ConfigurationManager.AppSettings["AuthClock"]);


            //数据库服务器重启，导致服务启动时连接不上数据库，后台进程30分钟重连一次
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        //数据库连接字符串
                        GlobalConfiguration.Configuration.UseSqlServerStorage("connStr");
                        _server = new BackgroundJobServer();

                        //每隔5分钟，每天6点-23点期间执行
                        RecurringJob.AddOrUpdate<UpDownJob>(x => x.ExecuteAsync(), upDownCron, TimeZoneInfo.Local);
                        //BackgroundJob.Schedule<UpDownJob>(x => x.ExecuteAsync(), TimeSpan.FromSeconds(15));

                        //每隔30分钟，每天6点-23点期间执行
                        //RecurringJob.AddOrUpdate<SyncStockJob>(x => x.ExecuteAsync(), syncStockCron, TimeZoneInfo.Local);
                        //BackgroundJob.Schedule<SyncStockJob>(x => x.ExecuteAsync(), TimeSpan.FromSeconds(15));

                        //每天早上8点执行
                        RecurringJob.AddOrUpdate<AuthJob>(x => x.ExecuteAsync(), Cron.Daily(authClock), TimeZoneInfo.Local);
                        //RecurringJob.AddOrUpdate<AuthJob>(x => x.ExecuteAsync(),Cron.Daily(16,8), TimeZoneInfo.Local);

                        break;
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(1800000);
                    }
                }
            });
        }

        protected override void OnStop()
        {
            _server.Dispose();
        }
    }
}
