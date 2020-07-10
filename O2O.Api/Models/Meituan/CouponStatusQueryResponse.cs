using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class CouponStatusQueryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double realAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dealType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dealId { get; set; }
        /// <summary>
        /// 团购名称123
        /// </summary>
        public string dealTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double dealPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double dealValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DealSkuMappingDetail dealSkuMappingDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dealPromotionMappingDetail { get; set; }
    }    
}