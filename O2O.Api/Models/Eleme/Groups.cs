using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class Groups
    {
        public string name { get; set; }

        public string type { get; set; }

        public List<Items> items { get; set; }
    }
}