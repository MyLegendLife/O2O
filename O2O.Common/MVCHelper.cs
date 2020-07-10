using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace O2O.Common
{
    public class MVCHelper
    {
        /// <summary>
        /// 服务端验证失败时候调用，可以看具体是哪个属性报错的
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GetValidMsg(ModelStateDictionary modelState)//有两个ModelStateDictionary类，别弄混乱了。要使用System.Web.Mvc下的
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count <= 0)
                {
                    continue;
                }
                sb.Append("属性【").Append(key).Append("】错误：");
                foreach (var modelError in modelState[key].Errors)
                {
                    sb.AppendLine(modelError.ErrorMessage);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 构造参数的方法
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string ToQueryString(NameValueCollection nvc)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in nvc.AllKeys)
            {
                string value = nvc[key];
                //EscapeDataString就是对特殊字符进行uri编码
                sb.Append(key).Append("=")
                    .Append(Uri.EscapeDataString(value)).Append("&");
            }
            return sb.ToString().Trim('&');//去掉最后一个多余的&
        }

        public static string RemoveQueryString(NameValueCollection nvc, string name)
        {
            NameValueCollection newNVC = new NameValueCollection(nvc);
            newNVC.Remove(name);
            return ToQueryString(newNVC);
        }

        public static string UpdateQueryString(NameValueCollection nvc,
            string name, string value)
        {
            NameValueCollection newNVC = new NameValueCollection(nvc);
            if (newNVC.AllKeys.Contains(name))
            {
                newNVC[name] = value;
            }
            else
            {
                newNVC.Add(name, value);
            }
            return ToQueryString(newNVC);
        }

        public static string RenderViewToString(ControllerContext context,
                string viewPath,
                object model = null)
        {
            ViewEngineResult viewEngineResult =
            ViewEngines.Engines.FindView(context, viewPath, null);
            if (viewEngineResult == null)
                throw new FileNotFoundException("View" + viewPath + "cannot be found.");
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                context.Controller.ViewData,
                                context.Controller.TempData,
                                sw);
                view.Render(ctx, sw);
                return sw.ToString();
            }
        }
    }
}
