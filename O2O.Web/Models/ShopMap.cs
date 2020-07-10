using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class ShopMap
    {
        public Guid AccountId { get; set; }

        public string AccessToken { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public long ShopId { get; set; }

        public string ShopNameEle { get; set; }

        public string ShopNo { get; set; }

        public string ShopName { get; set; }
    }
}