using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class OProduct
    {
        public OProduct()
        {
            properties = new OProperty();
        }

        public long itemId { set; get; }
        public long categoryId { set; get; }
        public OProperty properties { set; get; }
    }

    public class OProperty
    {
        public List<OMaterial> materials { set; get; }
        public string name { set; get; }
        public List<OSpec> specs { set; get; }
    }
}