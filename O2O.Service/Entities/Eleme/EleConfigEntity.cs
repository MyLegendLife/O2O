using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class EleConfigEntity : BaseEntity
    {
        /// <summary>
        /// 饿了么Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 饿了么Token到期后，更新时使用的Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 饿了么Token到期时间
        /// </summary>
        public DateTime ExpiresDate { get; set; }

        /// <summary>
        /// 饿了么区域标识
        /// </summary>
        public string  Sign  { get; set; }

        public Guid UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
