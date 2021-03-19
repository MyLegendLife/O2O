using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class StockRuleShop
    {
        public Guid? Id { get; set; }

        public string ShopNo { get; set; }

        public string ShopName { get; set; }

        public Guid? StockRuleId { get; set; }

        public string StockRuleName { get; set; }
    }
}