using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class MtOrderDtlEntity : BaseEntity
    {
        public string SkuId { get; set; }

        public string FoodName { get; set; }

        public int Quantity {get;set;}

        public int BoxNum { get; set; }

        public decimal BoxPrice { get; set; }

        public string Unit { get; set; }

        public decimal FoodDiscount { get; set; }

        public string Spec { get; set; }

        public string FoodPreperty { get; set; }

        public Guid OrderId { get; set; }

        public MtOrderEntity Order { get; set; }
    }
}
