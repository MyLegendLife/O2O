using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class MtAccountEditPost
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public string WaimaiAppId { get; set; }

        public string WaimaiAppSecret { get; set; }

        public string TuangouAppKey { get; set; }

        public string TuangouAppSecret { get; set; }

        public string Description { get; set; }
    }
}