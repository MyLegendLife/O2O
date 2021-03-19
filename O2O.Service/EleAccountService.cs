using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities.Eleme;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.Service
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

        public void Update(Ele_AccountDTO dto)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<Ele_AccountEntity>(context);

                var entity = ToolsCommon.EntityToEntity(dto, new Ele_AccountEntity()) as Ele_AccountEntity;

                service.Update(entity);
            }
        }

        public Ele_AccountDTO Get(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<Ele_AccountEntity>(context);

                Ele_AccountEntity entity = service.GetById(id);

                return ToolsCommon.EntityToEntity(entity,new Ele_AccountDTO()) as Ele_AccountDTO;
            }
        }

        public Ele_AccountDTO Get(string userNo)
        {
            throw new NotImplementedException();
        }

        public Ele_AccountDTO Get(string userId, string shopNo)
        {
            using (var context = new O2OContext())
            {
                var entity = from a in context.Ele_Account
                    join b in context.Ele_Shop on a.Id equals b.AccountId
                    where a.UserId == userId && b.ShopNo == shopNo
                    select a;

                return ToolsCommon.EntityToEntity(entity, new Ele_AccountDTO()) as Ele_AccountDTO;
            }
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

        public List<Ele_AccountDTO> GetExpiresAccounts(DateTime dateTime)
        {
            using (O2OContext context = new O2OContext())
            {
                var service = new BaseService<Ele_AccountEntity>(context);

                var list = service.Entities.
                    Where(a => a.ExpiresDate <= dateTime).
                    ToList().
                    Select(a => ToolsCommon.EntityToEntity(a, new Ele_AccountDTO()) as Ele_AccountDTO).
                    ToList();

                return list;
            }
        }
    }
}
