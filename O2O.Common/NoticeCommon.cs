using ModuBase;
using ModuData;
using ModuHandleBase;
using ModuRoute;
using System.Configuration;

namespace O2O.Common
{
    public class NoticeCommon
    {
        public static ConnectManag GetConnect(string userId)
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"].ToString();

            //商户信息
            RouteBusinMeta busin = RouteBusinOpera.GetBusinInfo(userId, secretKey);
            if (busin == null) return null;
            if (busin.AgentProxy == null) return null;

            ConnectFactory factory = new ConnectFactory(sys.separ(busin.AgentProxy));
            factory.SetBusin(busin.BusinNo, busin.BusinPass, "BuyTake");

            return new ConnectManag(factory, new CacheParam("BuyTake"));
        }

        public static TaskMeta NewTask(string SID, string ShopNo)
        {
            TaskMeta Task = new TaskMeta();
            Task.Topic = TaskTopic.BuyTake.ToString();
            Task.ShopNo = ShopNo;

            Task.OrderSID = SID;
            Task.OrderType = 0;

            return Task;
        }

        public static void Notice(string userId, string orderId, string shopNo)
        {
            var conn = GetConnect(userId);
            if (conn is null) return;

            TaskNotice Task = conn.GetNoticeTask();
            Task.SendTask(NewTask(orderId, shopNo));
        }
    }
}