using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class DealSkuMappingDetail
    {
        public int count { get; set; }

        public List<string> vendorSkus { get; set; }
    }
}