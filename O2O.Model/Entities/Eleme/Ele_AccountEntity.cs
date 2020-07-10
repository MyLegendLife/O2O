using System;
using System.Collections.Generic;

namespace O2O.Model
{
    public class Ele_AccountEntity : BaseEntity
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
        public DateTime ExpiresDate { get; set; }

        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }

        public virtual ICollection<Ele_ShopEntity> Shops { get; set; } = new  List<Ele_ShopEntity>();
    }
}
