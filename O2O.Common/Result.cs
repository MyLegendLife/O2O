using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Common
{
    /// <summary>
    /// 返回给365客户端的对象
    /// </summary>
    public class Result
    {
        public string State { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }

    /// <summary>
    /// 所有ajax都要返回这个类型的对象
    /// </summary>
    public class AjaxResult
    {
        // <summary>
        /// 执行的结果
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 执行返回的数据
        /// </summary>
        public object data { get; set; }
    }

    /// <summary>
    /// 饿了么返回的类型
    /// </summary>
    public class EleResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { set; get; }
        public string message { set; get; }
    }
}
