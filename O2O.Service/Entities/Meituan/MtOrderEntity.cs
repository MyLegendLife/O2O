using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class MtOrderEntity : BaseEntity
    {
        public string OrderId { get; set; }

        public string OrderIdView { get; set; }

        public string Caution { get; set; }

        public DateTime CTime { get; set; }

        public DateTime UTime { get; set; }

        public int DaySeq { get; set; }

        public DateTime DeliveryTime { get; set; }

        public decimal OriginalPrice { get; set; }

        public string RecipientAddress { get; set; }

        public string RecipientName { get; set; }

        public string RecipientPhone { get; set; }

        public string ShippingPhone { get; set; }

        public decimal ShippingFee { get; set; }

        public Status Status { get; set; }

        public decimal Total { get; set; }

        public SaledStatus IsSaled { get; set; }

        public ICollection<MtOrderDtlEntity> OrderDtls { get; set; } = new List<MtOrderDtlEntity>();
    }

    public enum Status
    { 
        A = 0,
        B= 1
    }

    public enum SaledStatus
    {
        No = 0,
        Yes = 1
    }
}
