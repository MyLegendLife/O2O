using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class FoodMap
    {
        public string Token { get; set; }

        public long CateId { get; set; }

        public string CateName { get; set; }

        public long ItemId { get; set; }

        public string ItemName { get; set; }

        public long SpecId { get; set; }

        public string SpecName { get; set; }

        public double Price { get; set; }

        public double Stock { get; set; }

        public string ProdNo { get; set; }

        public string ProdName { get; set; }

        public string ProdUnit { get; set; }

        public string SalePrice { get; set; }
    }
}