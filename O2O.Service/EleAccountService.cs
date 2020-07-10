using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.Service.NewFolder1
{
    public class EleAccountService : IEleAccountService
    {
        public void Add(Ele_AccountDTO dto)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<Ele_AccountEntity>(context);

                var entity = ToolsCommon.EntityToEntity(dto, new Ele_AccountEntity()) as Ele_AccountEntity;

                service.Add(entity);
            }
        }

        public bool Delete(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<Ele_AccountEntity>(context);

                try
                {
                    service.Delete(id);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public Ele_AccountDTO Get(string userNo)
        {
            throw new NotImplementedException();
        }

        public Ele_AccountDTO Get(string userNo, string shopNo)
        {
            throw new NotImplementedException();
        }

        public List<Ele_AccountDTO> GetAccounts(string userId)
        {
            using (O2OContext context = new O2OContext())
            {
                var service = new BaseService<Ele_AccountEntity>(context);

                 var list = service.Entities.
                    Where(a => a.UserId == userId).
                    ToList().
                    Select(a => ToolsCommon.EntityToEntity(a, new Ele_AccountDTO()) as Ele_AccountDTO).
                    ToList();

                return list;
            }
        }
    }
}
