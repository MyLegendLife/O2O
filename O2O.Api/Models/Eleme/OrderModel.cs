using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class OrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 上海市普陀区金沙江路丹巴路119号-NAPOS
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime createdAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime activeAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double deliverFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? deliverTime { get; set; }
        /// <summary>
        /// 爱吃辣多点辣
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Groups> groups { get; set; }
        /// <summary>
        /// 上海市拉拉队有限公司
        /// </summary>
        public string invoice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool book { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool onlinePaid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string railwayAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> phoneList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long shopId { get; set; }
        /// <summary>
        /// 测试餐厅001
        /// </summary>
        public string shopName { get; set; }
        /// <summary>g
        /// 
        /// </summary>
        public int daySn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ElemeEnum.State status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ElemeEnum.Refund refundStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double totalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double originalPrice { get; set; }
        /// <summary>
        /// 饿了么 先生
        /// </summary>
        public string consignee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deliveryGeo { get; set; }
        /// <summary>
        /// 上海市普陀区金沙江路丹巴路119号-NAPOS
        /// </summary>
        public string deliveryPoiAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool invoiced { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double income { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double serviceRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double serviceFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double hongbao { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double packageFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double activityTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double shopPart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double elemePart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool downgraded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double vipDeliveryFeeDiscount { get; set; }

        public UserExtraInfo userExtraInfo { get; set; }
    }

}