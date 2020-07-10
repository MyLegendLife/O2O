using O2O.Common;
using O2O.DTO;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class UserService : IUserService
    {
        public UserDTO Get(string id)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var entity = service.Entities.FirstOrDefault(a=> a.Id == id);

                return ToolsCommon.EntityToEntity(entity, new UserDTO()) as UserDTO;
            }
        }

        public UserDTO GetByLoginName(string loginName)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var entity = service.Entities.Where(a => a.LoginName == loginName).FirstOrDefault();

                return ToolsCommon.EntityToEntity(entity,new UserDTO()) as UserDTO;
            }
        }

        public void Add(UserDTO dto)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(context);

                var entity = ToolsCommon.EntityToEntity(dto,new UserEntity()) as UserEntity;

                service.Add(entity);
            }
        }

        public List<UserDTO> GetAll()
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
                var hash = ToolsCommon.MD5Encrypt(password + salt);//md5(—Œ+√‹¬Î)
                return user.PasswordHash == hash;
            }
        }
    }
}
