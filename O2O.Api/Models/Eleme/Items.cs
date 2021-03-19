using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class Items
    {
        public long id { get; set; }

        public long skuId { get; set; }

        public string name { get; set; }

        public long categoryId { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        public double total { get; set; }

        public List<string> additions { get; set; }

        public string extendCode { get; set; }

        public List<AttributesItem> attributes { get; set; }
    }

    public class AttributesItem
    {
        /// <summary>
        /// 温度
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 热
        /// </summary>
        public string value { get; set; }
    }
}