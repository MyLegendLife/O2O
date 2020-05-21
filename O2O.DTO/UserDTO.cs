using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class UserDTO: BaseDTO
    {
        public string UserNo { get; set; }

        public string UserName { get; set; }

        public string Description { get; set; }

        public MtConfigDTO MtConfig { get; set; }
    }
}
