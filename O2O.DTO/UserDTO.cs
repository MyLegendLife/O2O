using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string UserNo { get; set; }

        public string UserName { get; set; }

        public string LoginName { get; set; }

        public string ConnString { get; set; }

        public string Ket { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string SetBuyPara { get; set; }
    }
}
