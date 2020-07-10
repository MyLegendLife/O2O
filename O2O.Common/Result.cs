using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Common
{
    /// <summary>
    /// ���ظ�365�ͻ��˵Ķ���
    /// </summary>
    public class Result
    {
        public string State { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }

    /// <summary>
    /// ����ajax��Ҫ����������͵Ķ���
    /// </summary>
    public class AjaxResult
    {
        // <summary>
        /// ִ�еĽ��
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// ִ�з��ص�����
        /// </summary>
        public object data { get; set; }
    }

    /// <summary>
    /// ����ô���ص�����
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
