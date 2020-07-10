using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace O2O.Service
{
    public class EleShopService : IEleShopService
    {
        public void Add(Ele_ShopDTO dto)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<Ele_ShopEntity> service = new BaseService<Ele_ShopEntity>(context);

                var entity = ToolsCommon.EntityToEntity(dto, new Ele_ShopEntity()) as Ele_ShopEntity;

                service.Add(entity);
            }
        }

        public void Update(Ele_ShopDTO dto)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<Ele_ShopEntity> service = new BaseService<Ele_ShopEntity>(context);

                var entity = ToolsCommon.EntityToEntity(dto, new Ele_ShopEntity()) as Ele_ShopEntity;

                service.Update(entity);
            }
        }

        public bool Delete(long shopId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<Ele_ShopEntity> service = new BaseService<Ele_ShopEntity>(context);

                var entity = service.Where(a => a.ShopId == shopId).FirstOrDefault();
                try
                {
                    service.Delete(entity);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public Ele_ShopDTO Get(string userId, string shopNo)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<Ele_ShopEntity> service = new BaseService<Ele_ShopEntity>(context);

                var entity = service.Entities.FirstOrDefault(a => a.Account.User.Id == userId && a.ShopNo == shopNo);

                if (entity != null)
                {
                    var dto = ToolsCommon.EntityToEntity(entity, new Ele_ShopDTO()) as Ele_ShopDTO;

                    dto.AccessToken = entity.Account.AccessToken;
                    dto.UserId = userId;

                    return dto;
                }
                else
                {
                    return null;
                }
            }
        }

        public Ele_ShopDTO GetByShopId(long shopId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<Ele_ShopEntity> service = new BaseService<Ele_ShopEntity>(context);

                var entity = service.Where(a => a.ShopId == shopId).FirstOrDefault();

                if (entity == null) return null;

                var dto = ToolsCommon.EntityToEntity(entity, new Ele_ShopDTO()) as Ele_ShopDTO;

                dto.AccessToken = entity.Account.AccessToken;
                dto.UserId = entity.Account.User.Id;

                return dto;
            }
        }        

        public List<Ele_ShopDTO> GetByAccountId(Guid accountId)
        {
            using (O2OContext context = new O2OContext() )
            {
                BaseService<Ele_ShopEntity> service = new BaseService<Ele_ShopEntity>(context);

                var list = service.Entities.
                    Where(a=> a.AccountId == accountId).
                    ToList().
                    Select(a =>ToolsCommon.EntityToEntity(a, new Ele_ShopDTO()) as Ele_ShopDTO).
                    ToList();

                return list;
            }
        }

        public List<Ele_ShopDTO> GetByUserId(string userId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var user = service.Entities.Where(a => a.Id == userId).FirstOrDefault();

                var list = new List<Ele_ShopEntity>();
                foreach (var account in user.Ele_Accounts)
                {
                    list.AddRange(account.Shops);
                }

                return list.Select(a => ToolsCommon.EntityToEntity(a, new Ele_ShopDTO()) as Ele_ShopDTO).ToList();
            }
        }
    }
}
