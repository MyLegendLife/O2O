using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Model
{
    public class OrderDtlEntity : BaseEntity
    {
       public string ProdNo { set; get; }//单品编号
        public string ProdName { set; get; }//商品名称
        public string ProdUnit { set; get; }//单位
        public double Price { set; get; } //商品原价
        public double ItemCnt { set; get; }//数量
        public double ItemSum { set; get; }//金额

        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
