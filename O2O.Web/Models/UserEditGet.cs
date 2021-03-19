using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class UserEditGet
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string LoginName { get; set; }

        public string Ket { get; set; }

        public string Description { get; set; }
    }
}