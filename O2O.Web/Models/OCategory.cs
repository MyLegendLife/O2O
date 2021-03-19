using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Web.Models
{
    public class DayPartingStick
    {
        /// <summary>
        /// 
        /// </summary>
        public string beginDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TimesItem> times { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> weekdays { get; set; }
    }

    public class TimesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
    }

    public class OCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public string categoryType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string children { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DayPartingStick dayPartingStick { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isValid { get; set; }
        /// <summary>
        /// 食品安全，我们守护！（勿拍）
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long parentId { get; set; }
    }
}
