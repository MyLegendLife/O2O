using O2O.DTO.Eleme;
using O2O.DTO.Meituan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.IService
{
    public interface IEleAccountService : IServiceSupport
    {
        void Add(Ele_AccountDTO dto);

        bool Delete(Guid id);

        void Update(Ele_AccountDTO dto);

        Ele_AccountDTO Get(Guid id);

        Ele_AccountDTO Get(string userNo);

        Ele_AccountDTO Get(string userNo, string shopNo);

        List<Ele_AccountDTO> GetAccounts(string userId);
    }
}
