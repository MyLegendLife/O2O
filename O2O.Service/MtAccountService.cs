using O2O.Common;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using O2O.Model.Entities.Meituan;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.Service
{
    public class MtAccountService : IMtAccountService
    {
        public Mt_AccountDTO GetAccount(string userId)
        {
            using (var db = new O2OContext())
            {
                UserEntity userEntity = new BaseService<UserEntity>(db).Entities.FirstOrDefault(a => a.Id == userId);
                return userEntity != null ? ToolsCommon.EntityToEntity(userEntity.Mt_Accounts.FirstOrDefault(), new Mt_AccountDTO()) as Mt_AccountDTO : (Mt_AccountDTO)null;
            }
        }

        public Mt_AccountDTO GetAccount(Guid id)
        {
            using (var db = new O2OContext())
            {
                var mtAccountEntity = new BaseService<Mt_AccountEntity>(db).FirstOrDefault(a => a.Id == id);
                return mtAccountEntity == null ? null : ToolsCommon.EntityToEntity(mtAccountEntity, new Mt_AccountDTO()) as Mt_AccountDTO;
            }
        }

        public List<Mt_AccountDTO> GetAccountAll()
        {
            using (var db = new O2OContext())
                return new BaseService<Mt_AccountEntity>(db).GetAll().ToList().Select(a => ToolsCommon.EntityToEntity(a, new Mt_AccountDTO()) as Mt_AccountDTO).ToList();
        }

        public List<Mt_AccountDTO> GetList(string userId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<Mt_AccountEntity>(context);

                return service
                    .Where(a => a.UserId == userId)
                    .ToList()
                    .Select(a => ToolsCommon.EntityToEntity(a, new Mt_AccountDTO()) as Mt_AccountDTO)
                    .ToList();
            }
        }

        public void Add(
          string userId,
          string accountNo,
          string accountName,
          string waimaiAppId,
          string waimaiAppSecret,
          string tuangouAppKey,
          string tuangouAppSecret,
          string description)
        {
            var entity = new Mt_AccountEntity()
            {
                UserId = userId,
                AccountNo = accountNo,
                AccountName = accountName,
                WaimaiAppId = waimaiAppId,
                WaimaiAppSecret = waimaiAppSecret,
                TuangouAppKey = tuangouAppKey,
                TuangouAppSecret = tuangouAppSecret,
                Description = description
            };
            using (var db = new O2OContext())
                new BaseService<Mt_AccountEntity>(db).Add(entity);
        }

        public void Update(
          Guid id,
          string userId,
          string accountNo,
          string accountName,
          string waimaiAppId,
          string waimaiAppSecret,
          string tuangouAppKey,
          string tuangouAppSecret,
          string description)
        {
            var mtAccountEntity = new Mt_AccountEntity()
            {
                UserId = userId,
                AccountNo = accountNo,
                AccountName = accountName,
                WaimaiAppId = waimaiAppId,
                WaimaiAppSecret = waimaiAppSecret,
                TuangouAppKey = tuangouAppKey,
                TuangouAppSecret = tuangouAppSecret,
                Description = description
            };
            using (O2OContext db = new O2OContext())
            {
                BaseService<Mt_AccountEntity> baseService = new BaseService<Mt_AccountEntity>(db);
                Mt_AccountEntity entity = baseService.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentNullException("’Àªß≤ª¥Ê‘⁄");
                entity.UserId = userId;
                entity.AccountNo = accountNo;
                entity.AccountName = accountName;
                entity.WaimaiAppId = waimaiAppId;
                entity.WaimaiAppSecret = waimaiAppSecret;
                entity.TuangouAppKey = tuangouAppKey;
                entity.TuangouAppSecret = tuangouAppSecret;
                entity.Description = description;
                baseService.Update(entity);
            }
        }

        public void Delete(Guid id)
        {
            using (O2OContext db = new O2OContext())
                new BaseService<Mt_AccountEntity>(db).Delete(id);
        }
    }
}
