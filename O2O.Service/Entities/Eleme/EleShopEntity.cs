using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class EleShopEntity : BaseEntity
    {
        /// <summary>
        /// 饿了么门店Id
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 饿了么门店名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 365门店编号
        /// </summary>
        public string ShopNo { get; set; }

        /// <summary>
        /// 饿了么区域标识
        /// </summary>
        public string  Sign  { get; set; }

        public Guid UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
