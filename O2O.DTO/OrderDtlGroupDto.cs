using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO
{
    public class OrderDtlGroupDto
    {
        public string ProdNo { set; get; }

        public string ProdName { set; get; }

        public string ProdUnit { set; get; }

        public double Price { set; get; } //商品原价 
    }
}
