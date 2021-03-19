using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.Service
{
    public class UserService : IUserService
    {
        public UserDTO Get(string id)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var entity = service.Entities.FirstOrDefault(a => a.Id == id);

                return ToolsCommon.EntityToEntity(entity, new UserDTO()) as UserDTO;
            }
        }

        public UserDTO GetByLoginName(string loginName, string id = "")
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var entity = service.Entities.FirstOrDefault(a => a.LoginName == loginName && (id == "" || a.Id != id));

                return ToolsCommon.EntityToEntity(entity, new UserDTO()) as UserDTO;
            }
        }

        public void Add(UserDTO dto)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var entity = ToolsCommon.EntityToEntity(dto, new UserEntity()) as UserEntity;

                service.Add(entity);
            }
        }

        public void Add(
      string id,
      string userName,
      string loginName,
      string password,
      string connString,
      string ket,
      string description)
        {
            UserEntity entity = new UserEntity()
            {
                Id = id,
                UserName = userName,
                LoginName = loginName,
                PasswordSalt = ToolsCommon.CreateVerifyCode(5)
            };
            entity.PasswordHash = ToolsCommon.MD5Encrypt(password + entity.PasswordSalt);
            entity.ConnString = ToolsCommon.ToBase64(connString);
            entity.Ket = ket;
            entity.Description = description;
            entity.CreateDate = DateTime.Now;
            using (O2OContext db = new O2OContext())
                new BaseService<UserEntity>(db).Add(entity);
        }

        public void Update(
          string id,
          string userName,
          string loginName,
          string password,
          string connString,
          string ket,
          string description)
        {
            using (O2OContext db = new O2OContext())
            {
                BaseService<UserEntity> baseService = new BaseService<UserEntity>(db);
                UserEntity entity = baseService.FirstOrDefault(a => a.Id == id) ?? throw new ArgumentNullException("商户不存在");
                entity.Id = id;
                entity.UserName = userName;
                entity.LoginName = loginName;
                if (!string.IsNullOrWhiteSpace(password))
                    entity.PasswordHash = ToolsCommon.MD5Encrypt(password + entity.PasswordSalt);
                if (!string.IsNullOrWhiteSpace(connString))
                    entity.ConnString = ToolsCommon.ToBase64(connString);
                entity.Ket = ket;
                entity.Description = description;
                baseService.Update(entity);
            }
        }

        public void Delete(string id)
        {
            using (O2OContext db = new O2OContext())
                new BaseService<UserEntity>(db).Delete(id);
        }

        public List<UserDTO> GetAll(string para = "")
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                return service.Entities.
                    ToList().
                    Select(a => ToolsCommon.EntityToEntity(a, new UserDTO()) as UserDTO).
                    ToList();
            }
        }

        public bool CheckLogin(string loginName, string password)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);
                var user = service.GetAll().SingleOrDefault(m => m.LoginName == loginName);
                if (user == null)
                {
                    return false;
                }
                var salt = user.PasswordSalt;
                var hash = ToolsCommon.MD5Encrypt(password + salt);//md5(盐+密码)
                return user.PasswordHash == hash;
            }
        }
    }
}
