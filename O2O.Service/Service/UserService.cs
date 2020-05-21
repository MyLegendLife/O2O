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
        
        public UserService(DbContext db)
        { 
            
        }

        public IQueryable<UserDTO> GetAll()
        {
            using (O2OContext db = new O2OContext())
            {
                //db.get
            }


            throw new NotImplementedException();
        }

        private UserDTO ToDTO(UserEntity e)
        {
            return null; 
        }
    }
}
