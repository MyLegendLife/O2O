using log4net;
using System.Web.Mvc;

namespace O2O.Api.App_Start
{
    public class O2OExceptionFilter : IExceptionFilter
    {
        //声明Log4NET对象，建议一个类就声明一个ILog对象
        private static ILog log = LogManager.GetLogger(typeof(O2OExceptionFilter));

        public void OnException(ExceptionContext context)
        {
            log.ErrorFormat("出现未处理的异常{0}", context.Exception);
        }
    }
}