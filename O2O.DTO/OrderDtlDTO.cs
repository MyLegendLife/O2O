using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class OrderDtlDTO : BaseDTO
    {
        public string ProdNo { set; get; }//��Ʒ���
        public string ProdName { set; get; }//��Ʒ����
        public string ProdUnit { set; get; }//��λ
        public double Price { set; get; } //��Ʒԭ��
        public double ItemCnt { set; get; }//����
        public double ItemSum { set; get; }//���
        public double RefundPartCnt { get; set; } //�����˿�����
    }
}