using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class OrderQueryDTO : BaseDTO
    {
        public string UserId { get; set; }

        public int TakeType { set; get; }

        public string ShopNo { get; set; }

        public int OrderCount { get; set; }

        public int MaxDaySeq { get; set; }

        public int SetBuyCount { get; set; }

        public double Total { get; set; }

        public double Consume { get; set; }
    }
}
