using O2O.Model.Entities.Eleme;
using O2O.Model.Entities.Meituan;
using System;
using System.Collections.Generic;

namespace O2O.Model.Entities
{
    public class UserEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string ConnString { get; set; }
        public string Ket { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        //生成销售单的规则判断
        public string SetBuyPara { get; set; }

        public virtual ICollection<Mt_AccountEntity> Mt_Accounts { get; set; } = new List<Mt_AccountEntity>();
        public virtual ICollection<Ele_AccountEntity> Ele_Accounts { get; set; } = new List<Ele_AccountEntity>();
    }
}
