using O2O.DTO.Meituan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.IService
{
    public interface IMtAccountService : IServiceSupport
    {
        AccountDTO GetAccount(string userNo);

        AccountDTO GetAccount(string userNo,string shopNo);
    }
}
