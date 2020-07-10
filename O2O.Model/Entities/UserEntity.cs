using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Model
{
    public class UserEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string ConnString { get; set; }
        public string Ket { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Mt_AccountEntity> Mt_Accounts { get; set; } = new List<Mt_AccountEntity>();
        public virtual ICollection<Ele_AccountEntity> Ele_Accounts { get; set; } = new List<Ele_AccountEntity>();
    }
}
