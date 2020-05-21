using O2O.DTO;
using System.Linq;

namespace O2O.IService
{
    public interface IUserService
    {
        IQueryable<UserDTO> GetAll();
    }
}
