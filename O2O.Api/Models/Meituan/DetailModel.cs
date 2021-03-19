using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class DetailModel
    {
        public string app_food_code { get; set; }

        public string food_name { get; set; }

        public string sku_id { get; set; }

        public double quantity { get; set; }

        public double price { get; set; }

        public double box_num { get; set; }

        public double box_price { get; set; }

        public string unit { get; set; }

        public double food_discount { get; set; }

        public string food_property { get; set; }

        public string spec { get; set; }

        public string cart_id { get; set; }
    }
}