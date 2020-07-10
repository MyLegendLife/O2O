using O2O.DTO;
using O2O.DTO.Meituan;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.IService
{
    public interface IUserService : IServiceSupport
    {
        UserDTO Get(string id);

        UserDTO GetByLoginName(string loginName);

        void Add(UserDTO dto);

        List<UserDTO> GetAll();

        /// <summary>
        /// �����û��Ƿ��¼�ɹ�
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckLogin(string phoneNum, string password);
    }
}
