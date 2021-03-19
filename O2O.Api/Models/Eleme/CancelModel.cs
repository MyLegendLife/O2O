using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class CancelModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ElemeEnum.Refund refundStatus { get; set; }
        /// <summary>
        /// 用户申请退单
        /// </summary>
        public string reason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long shopId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refundType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double totalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GoodsList> goodsList { get; set; }
    }

}