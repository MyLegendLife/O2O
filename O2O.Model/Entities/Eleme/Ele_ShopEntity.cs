using System;

namespace O2O.Model.Entities.Eleme
{
    public class Ele_ShopEntity : BaseEntity
    {
        public string ShopNo { get; set; }

        public long ShopId { get; set; }

        public Guid AccountId { get; set; }

        public virtual Ele_AccountEntity Account { get; set; }
    }
}
