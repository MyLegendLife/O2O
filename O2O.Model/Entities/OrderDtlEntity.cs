using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Model.Entities
{
    public class OrderDtlEntity : BaseEntity
    {
       public string ProdNo { set; get; }//��Ʒ���
        public string ProdName { set; get; }//��Ʒ����
        public string ProdUnit { set; get; }//��λ
        public double Price { set; get; } //��Ʒԭ��
        public double ItemCnt { set; get; }//����
        public double ItemSum { set; get; }//���

        public double RefundPartCnt { get; set; } //�����˿�����

        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
