using System;

namespace O2O.Model
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
