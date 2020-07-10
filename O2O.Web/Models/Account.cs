using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class Account
    {
        public string Id { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ExpiresDate { get; set; }

        public string UserId { get; set; }
    }
}