using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
