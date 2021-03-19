using O2O.Common;
using O2O.DTO.Meituan;
using O2O.IService;
using System.Linq;

namespace O2O.Service.Meituan
{
    public class MtBaseApiService
    {
        public string _waimaiAppId;
        public string _waimaiAppSecret;
        public string _shopNo;

        public MtBaseApiService(string userId, string shopNo)
        {
            IMtAccountService service = new MtAccountService();
            Mt_AccountDTO account = service.GetAccount(userId);
            if (account is null) return;

            _waimaiAppId = account.WaimaiAppId;
            _waimaiAppSecret = account.WaimaiAppSecret;
            _shopNo = shopNo;
        }

        public string GetUrl(string url, object model)
        {
            var sort = model.GetType().GetProperties().OrderBy(a => a.Name);

            string str = "";
            foreach (var item in sort)
            {
                str += item.Name + "=" + item.GetValue(model, null) + "&";
            }
            str = str.TrimEnd('&');

            string sig = ToolsCommon.MD5Encrypt(url + "?" + str + _waimaiAppSecret);

            return url + "?" + str + "&sig=" + sig;
        }
    }
}