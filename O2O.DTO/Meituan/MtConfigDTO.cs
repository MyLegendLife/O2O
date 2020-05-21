using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class MtConfigDTO : BaseDTO
    {
        public string WaimaiAppId { get; set; }

        public string WaimaiAppSecret { get; set; }

        public string TuangouAppKey { get; set; }

        public string TuangouAppSecret { get; set; }
    }
}
