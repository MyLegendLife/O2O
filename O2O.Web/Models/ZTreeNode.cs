using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    /// <summary>
    /// 节点实体模型类
    /// </summary>
    public class ZTreeNode
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string iconOpen { get; set; }
        public string iconClose { get; set; }
        /// <summary>
        /// 展开
        /// </summary>
        public bool open { get; set; }
        /// <summary>
        /// 没有子节点
        /// </summary>
        public bool isParent { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string name { get; set; }

        public string icon { get; set; }

        public string token { get; set; }
    }
}