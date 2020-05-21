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
        BaseService<UserEntity> _service;

        public UserService(O2OContext db)
        {
            _service = new BaseService<UserEntity>(db);
        }

        public List<UserDTO> GetAll()
        {
            List<UserDTO> list = new List<UserDTO>();
            foreach (var entity in _service.Entities)
            {
                list.Add(ToDTO(entity));
            }

            return list;
        }

        private UserDTO ToDTO(UserEntity entity)
        {
            UserDTO dto = new UserDTO()
            {
                UserNo = entity.UserNo,
                UserName= entity.UserName,
            };

            return dto;
        }
    }
}
