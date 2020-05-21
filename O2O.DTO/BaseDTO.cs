using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class BaseDTO
    {
        public Guid Id { get; set; }

        public DateTime CreateDateTime{ get; set; }
    }
}
