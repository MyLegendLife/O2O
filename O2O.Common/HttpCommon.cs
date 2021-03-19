using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace O2O.Common
{
    public class HttpCommon
    {
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">Url地址</param>
        public static string Get(string url)
        {
            var request = (WebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            var result = string.Empty;
            using (var response = request.GetResponse())
            {
                if (response != null)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string Post(string url)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            var resp = (HttpWebResponse)req.GetResponse();
            var stream = resp.GetResponseStream();
            //获取内容
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string PostJObject(string url, JObject model)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "x-www-form-urlencoded";

            #region 添加Post 参数
            var content = JsonConvert.SerializeObject(model);

            var data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            var resp = (HttpWebResponse)req.GetResponse();
            var stream = resp.GetResponseStream();
            //获取响应内容
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static async Task<string> PostJObjectAsync(string url, JObject model)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "x-www-form-urlencoded";

            #region 添加Post 参数
            var content = JsonConvert.SerializeObject(model);

            var data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            var resp = (HttpWebResponse)(await req.GetResponseAsync());
            var stream = resp.GetResponseStream();
            //获取响应内容
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string Post(string url,object model)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "x-www-form-urlencoded";

            #region 添加Post 参数
            var content = JsonConvert.SerializeObject(model);

            var data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            var resp = (HttpWebResponse)req.GetResponse();
            var stream = resp.GetResponseStream();
            //获取响应内容
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> dic)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "x-www-form-urlencoded";
            #region 添加Post 参数
            var builder = new StringBuilder();
            var i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            var data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            var resp = (HttpWebResponse)req.GetResponse();
            var stream = resp.GetResponseStream();
            //获取响应内容
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }


        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <param name="content">Post提交数据内容(utf-8编码的)</param>
        /// <returns></returns>
        public static string Post(string url, string content)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "x-www-form-urlencoded";

            #region 添加Post 参数
            var data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            var resp = (HttpWebResponse)req.GetResponse();
            var stream = resp.GetResponseStream();
            //获取响应内容
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Post(string url, string contentType, Dictionary<string, string> headers, string content)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = contentType;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    req.Headers.Add(item.Key, item.Value);
                }
            }

            #region 添加Post 参数
            var data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            try
            {
                var resp = (HttpWebResponse)req.GetResponse();
                var stream = resp.GetResponseStream();
                //获取响应内容
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }

                return result;
            }
            catch (WebException ex)
            {
                var mResponse = ex.Response as HttpWebResponse;
                var responseStream = mResponse.GetResponseStream();
                if (responseStream != null)
                {
                    var streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                    //获取返回的信息
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();
                }
                return result;
            }
        }

        public static string PostReceive(object objContext)
        {
            var context = (HttpContext)objContext;

            var res = "";
            var stream = context.Request.InputStream;
            if (stream != null)
            {
                if (stream.Length > 10)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        res = reader.ReadToEnd();
                    }
                }
            }

            //两次decode
            res = System.Web.HttpUtility.UrlDecode(res);
            return System.Web.HttpUtility.UrlDecode(res);
        }

        public static string GetReceive(object objContext)
        {
            var context = (HttpContext)objContext;

            return context.Request.Url.ToString();
        }
    }
}
