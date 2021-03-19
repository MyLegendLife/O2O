using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class CouponCode
    {
        public string couponCode { get; set; }

        public int dealType { get; set; }

        public double dealCouponAmount { get; set; }

        public double realAmount { get; set; }

        public double discountAmount { get; set; }

        public int dealId { get; set; }

        public string dealTitle { get; set; }

        public DealSkuMappingDetail dealSkuMappingDetail { get; set; }

        public string dealPromotionMappingDetail { get; set; }

        public DealSkuMappingDetail giftSkuMappingDetail { get; set; }

        public List<CouponSku> couponSkus { get; set; }
    }
}