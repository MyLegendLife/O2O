using System;

namespace O2O.DTO
{
    //打印用 里面的送货时间是string类型
    public class OrderInfoDTO : BaseDTO
    {
        public string UserId { get; set; }  //用户Id
        public int TakeType { set; get; } //外卖平台
        public string OrderId { set; get; }  //订单编号
        public double TtlPrice { set; get; } //消费总额
        public double Consume { set; get; } //应付金额
        public string UserName { set; get; } //用户姓名
        public string UserMobile { set; get; } //用户电话
        public string DeliverTime { set; get; } //送货时间
        public string DeliverAddress { set; get; } //送货地址
        public double DeliverFee { set; get; } //配送费用
        public string MemoStr { set; get; } //备注
        public DateTime OptTime { set; get; } //开单时间
        public int PayType { set; get; } //付款方式
        public int State { set; get; } //订单状态
        public int DaySeq { get; set; } //流水号

        public string CancelCode { get; set; }  //取消状态
        public string CancelReason { get; set; } //取消原因

        public string RefundCode { get; set; }  //退款状态
        public string RefundReason { get; set; }  //退款原因

        public int BuyState { get; set; } //365状态
        public int OrderType { get; set; } //0现购  1预订

        public double RefundPartAmt { get; set; } //部分退款金额

        public string DispatcherName { get; set; }  //骑手姓名
        public string DispatcherMobile { get; set; }  //骑手姓名
    }
}