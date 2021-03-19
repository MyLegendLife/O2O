using log4net;
using Newtonsoft.Json.Linq;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Service.Eleme;
using System;
using System.Threading.Tasks;

namespace O2O.BackgroundJobs.Jobs
{
    public class AuthJob
    {
        private readonly IEleAccountService _eleAccountService;
        private static ILog _log = LogManager.GetLogger("UpDownJob");

        public AuthJob(IEleAccountService eleAccountService)
        {
            _eleAccountService = eleAccountService;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                //获取5天后过期的账户
                var accounts = _eleAccountService.GetExpiresAccounts(DateTime.Now.AddDays(5));

                foreach (var account in accounts)
                {
                    await Auth(account);
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat($"【系统错误】：{e.Message}");
            }
        }

        private async Task Auth(Ele_AccountDTO dto)
        {
            var service = new EleUserApiService();

            var res = service.RefreshToken(dto.RefreshToken);

            if (res == "")
            {
                _log.DebugFormat("【更新授权异常】：返回结果空"); 
                return;
            }

            var jo = JObject.Parse(res);

            if (jo["error"] != null)
            {
                _log.DebugFormat($"【更新授权异常】：{jo["error_description"]}");
                return;
            }

            dto.AccessToken = jo["access_token"]?.ToString();
            dto.RefreshToken = jo["refresh_token"]?.ToString();
            dto.ExpiresDate = DateTime.Now.AddSeconds(double.Parse(jo["expires_in"]?.ToString() ?? string.Empty));

            _eleAccountService.Update(dto);
        }
    }
}
