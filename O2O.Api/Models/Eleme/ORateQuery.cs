using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Api.Models.Eleme
{
    public class ORateQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public long shopId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> starRating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int offset { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public string dataType { get; set; }
    }
}