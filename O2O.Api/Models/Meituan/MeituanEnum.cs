using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Meituan
{
    public class MeituanEnum
    {
        public enum State
        {
            [Display(Order = 0)]
            [Description("用户已提交订单")]
            One = 1,

            [Display(Order = 0)]
            [Description("向商家推送订单")]
            Two = 2,

            [Display(Order = 1)]
            [Description("商家已收到")]
            Three = 3,

            [Display(Order = 1)]
            [Description("商家已确认")]
            Four = 4,

            [Display(Order = 1)]
            [Description("订单配送中")]
            Six = 6,

            [Display(Order = 1)]
            [Description("订单已送达")]
            Seven = 7,

            [Display(Order = 3)]
            [Description("订单已完成")]
            Eight = 8,

            [Display(Order = 4)]
            [Description("订单已取消")]
            Nine = 9,
        }

        public enum CancelMt
        {
            [Description("系统取消，超时未确认")]
            A = 1001,

            [Description("系统取消，在线支付订单15分钟未支付")]
            B = 1002,

            [Description("用户取消，在线支付中取消")]
            C = 1101,

            [Description("用户取消，商家确认前取消")]
            D = 1102,

            [Description("用户取消，用户退款取消")]
            E = 1103,

            [Description("客服取消，用户下错单")]
            F = 1201,

            [Description("客服取消，用户测试")]
            G = 1202,

            [Description("客服取消，重复订单")]
            H = 1203,

            [Description("客服取消，其他原因")]
            I = 1204,

            [Description("其他原因")]
            J = 1301
        }

        public enum Cancel
        {
            [Description("APP方商家超时接单")]
            A = 2001,

            [Description("APP方非顾客原因修改订单")]
            B = 2002,

            [Description("APP方非顾客原因取消订单")]
            C = 2003,

            [Description("APP方配送延迟")]
            D = 20014,

            [Description("APP方售后投诉")]
            E = 2005,

            [Description("APP方用户要求取消")]
            F = 2006,

            [Description("APP方其他原因取消")]
            G = 2007,

            [Description("店铺太忙")]
            H = 2008,

            [Description("商品已售完")]
            I = 2009,

            [Description("地址无法配送")]
            J = 2010,

            [Description("店铺已打烊")]
            K = 2011,

            [Description("联系不上用户")]
            U = 2012,

            [Description("重复订单")]
            L = 2013,

            [Description("配送员取餐慢")]
            M = 2014,

            [Description("配送员送餐慢")]
            N = 2015,

            [Description("配送员丢餐、少餐、餐洒")]
            O = 2016,
        }

        public enum Refund
        {
            [Display(Order = 0)]
            [Description("发起退款")]
            part,

            [Display(Order = 0)]
            [Description("发起退款")]
            apply,

            [Display(Order =3)]
            [Description("确认退款")]
            agree,

            [Display(Order = 4)]
            [Description("驳回退款")]
            reject ,

            [Display(Order = 4)]
            [Description("用户取消退款申请")]
            cancelRefund,
        }
    }
}