using O2O.DTO;
using System.Collections.Generic;

namespace O2O.IService
{
    public interface IUserService : IServiceSupport
    {
        UserDTO Get(string id);

        UserDTO GetByLoginName(string loginName, string id = "");

        void Add(UserDTO dto);

        void Add(
          string id,
          string userName,
          string loginName,
          string password,
          string connString,
          string ket,
          string description);

        void Update(
          string id,
          string userName,
          string loginName,
          string password,
          string connString,
          string ket,
          string description);

        void Delete(string id);

        List<UserDTO> GetAll(string para = "");

        bool CheckLogin(string phoneNum, string password);
    }
}
