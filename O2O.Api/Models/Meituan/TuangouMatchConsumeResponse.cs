using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
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