using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class GoodsList
    {
        /// <summary>
        /// 黑椒牛柳意大利面[重辣]
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double price { get; set; }
    }

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