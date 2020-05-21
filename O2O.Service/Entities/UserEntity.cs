using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class UserEntity : BaseEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnString { get; set; }

        /// <summary>
        /// 365密钥
        /// </summary>
        public string Ket { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public Guid MtConfigId { get; set; }
        public virtual MtConfigEntity MtConfig { get; set; }

        public virtual ICollection<MtShopEntity> MtShops { get; set; } = new List<MtShopEntity>();

        public virtual ICollection<EleConfigEntity> EleConfigs { get; set; } = new List<EleConfigEntity>();

        public virtual ICollection<EleShopEntity> EleShops { get; set; } = new List<EleShopEntity>();
    }
}
