using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class ElemeEnum
    {
        //订单状态
        public enum State
        {
            [Display(Order = 0)]
            [Description("未生效订单")]
            pending ,

            [Display(Order = 0)]
            [Description("未处理订单")]
            unprocessed,

            [Display(Order = 0)]
            [Description("退单处理中")]
            refunding ,

            [Display(Order = 1)]
            [Description("已处理的有效订单")]
            valid,

            [Display(Order = 2)]
            [Description("无效订单")]
            invalid,

            [Display(Order = 4)]
            [Description("已完成订单")]
            settled,
        }

        //取消订单原因
        public enum Cancel
        {
            [Description("其他原因")]
            others,

            [Description("用户信息错误")]
            fakeOrder,

            [Description("联系不上用户")]
            contactUserFailed,

            [Description("商品已经售完")]
            foodSoldOut,

            [Description("商家已经打烊")]
            restaurantClosed,

            [Description("超出配送范围")]
            distanceTooFar,

            [Description("商家现在太忙")]
            restaurantTooBusy,

            [Description("用户要求取消")]
            forceRejectOrder,

            [Description("暂时无法配送")]
            deliveryFault,

            [Description("不满足起送要求")]
            notSatisfiedDeliveryRequirement,

            [Description("无骑手接单")] 
            noRiderOrder,

            [Description("骑手取餐慢")]
            riderTakeSlowMeal,

            [Description("骑手不愿意送餐、态度差")]
            riderReluctantDeliverMeal
        }
        
        //订单退款状态
        public enum Refund
        {
            [Display(Order = 4)]
            [Description("未申请退单")]
            noRefund,

            [Display(Order = 0)]
            [Description("用户申请退单")]
            applied,

            [Display(Order =4)]
            [Description("店铺拒绝退单")]
            rejected,

            [Display(Order = 0)]
            [Description("客服仲裁中")]
            arbitrating,

            [Display(Order = 4)]
            [Description("退单失败")]
            failed,

            [Display(Order = 3)]
            [Description("退单成功")]
            successful
        }
    }
}