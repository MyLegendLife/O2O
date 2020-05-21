using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class MtShopEntity : BaseEntity
    {
        /// <summary>
        /// 美团门店Id
        /// </summary>
        public string PoiId { get; set; }

        /// <summary>
        /// 美团门店名称
        /// </summary>
        public string PoiName { get; set; }

        /// <summary>
        /// 美团Token
        /// </summary>
        public string AppAuthToken { get; set; }

        /// <summary>
        /// 业务类型 1:团购 2:外卖
        /// </summary>
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 365门店编号
        /// </summary>
        public string ShopNo { get; set; }

        public Guid UserId { get; set; }

        public virtual UserEntity User { get; set; }

        /// <summary>
        /// 格式:用户编号_门店编号
        /// </summary>
        //[NotMapped]
        //public string EPoild
        //{
        //    get
        //    {
        //        return this.User.UserNo + "_" + this.ShopNo;
        //    }
        //    set 
        //    {
        //        EPoild = value;
        //    }
        //}
    }

    public enum BusinessType
    {
        Waimai = 1,
        Tuangou = 2
    }
}
