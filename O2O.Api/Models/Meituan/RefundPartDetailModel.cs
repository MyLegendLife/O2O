using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class RefundPartDetailModel
    {
        public string app_food_code { get; set; }

        public double box_num { get; set; }

        public double box_price { get; set; }

        public double count { get; set; }

        public string food_name { get; set; }

        public double food_price { get; set; }

        public double origin_food_price { get; set; }

        public double refund_price { get; set; }

        public string sku_id { get; set; }

        public string spec { get; set; }
    }
}