using O2O.DTO;
using O2O.IService;
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
        //public UserService(O2OContext db)
        //{
        //    _service = new BaseService<UserEntity>(db);
        //}

        public List<UserDTO> GetAll()
        {
            using (O2OContext db = new O2OContext())
            {
                BaseService<UserEntity>  service = new BaseService<UserEntity>(db);

                return service.Entities.ToList().Select(a => ToDTO(a)).ToList();
            }
        }

        public UserEntity Add(UserEntity entity)
        {
            using (O2OContext db = new O2OContext())
            {
                BaseService<UserEntity> service = new BaseService<UserEntity>(db);

                return service.Add(entity);
            }
        }

        private UserDTO ToDTO(UserEntity entity)
        {
            UserDTO dto = new UserDTO()
            {
                Id = entity.Id,
                UserNo = entity.UserNo,
                UserName = entity.UserName
            };

            return dto;
        }
    }
}
