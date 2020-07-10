using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class DealSkuMappingDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> vendorSkus { get; set; }
    }

    public class CouponSku
    {
        /// <summary>
        /// 
        /// </summary>
        public string vendorSkuId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
    }

    public class CouponCode
    {
        /// <summary>
        /// 
        /// </summary>
        public string couponCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dealType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double dealCouponAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double realAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double discountAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dealId { get; set; }
        /// <summary>
        /// 三人99套餐
        /// </summary>
        public string dealTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DealSkuMappingDetail dealSkuMappingDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dealPromotionMappingDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DealSkuMappingDetail giftSkuMappingDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CouponSku> couponSkus { get; set; }
    }

    public class TuangouMatchConsumeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public double toPayAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double totalDealCouponAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double totalRealAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double totalDiscountAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double tailAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CouponCode> couponCodeList { get; set; }
    }
}