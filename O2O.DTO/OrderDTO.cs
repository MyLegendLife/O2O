using System;

namespace O2O.DTO
{
    public class OrderDTO : BaseDTO
    {
        public string UserId { get; set; }  //�û�Id
        public int TakeType { set; get; } //����ƽ̨
        public string OrderId { set; get; }  //�������
        public double TtlPrice { set; get; } //�����ܶ�
        public double Consume { set; get; } //Ӧ�����
        public string UserName { set; get; } //�û�����
        public string UserMobile { set; get; } //�û��绰
        public DateTime DeliverTime { set; get; } //�ͻ�ʱ��
        public string DeliverAddress { set; get; } //�ͻ���ַ
        public double DeliverFee { set; get; } //���ͷ���
        public string MemoStr { set; get; } //��ע
        public DateTime OptTime { set; get; } //����ʱ��
        public int PayType { set; get; } //���ʽ
        public int State { set; get; } //����״̬
        public int DaySeq { get; set; } //��ˮ��

        public string CancelCode { get; set; }  //ȡ��״̬
        public string CancelReason { get; set; } //ȡ��ԭ��

        public string RefundCode { get; set; }  //�˿�״̬
        public string RefundReason { get; set; }  //�˿�ԭ��

        public int BuyState { get; set; } //365״̬
    }
}