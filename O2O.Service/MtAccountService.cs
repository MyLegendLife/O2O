using O2O.Common;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class MtAccountService : IMtAccountService
    {
        public AccountDTO GetAccount(string userId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var user = service.Entities.Where(a => a.Id == userId).FirstOrDefault();

                if (user != null)
                {
                    var account = user.Mt_Accounts.FirstOrDefault();
                    return ToolsCommon.EntityToEntity(account, new AccountDTO()) as AccountDTO;
                }

                return null;
            }
        }

        public AccountDTO GetAccount(string userNo, string shopNo)
        {
            throw new NotImplementedException();
        }
    }
}
