using ModuBase;
using ModuData;
using ModuHandleBase;
using ModuHandleBuy;
using ModuInterBuy;
using ModuMiddle;
using ModuRoute;
using O2O.Common;
using O2O.IService;
using O2O.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace O2O.Service
{
    public class Bak365Service
    {
        public Bak365Service()
        {
        }

        public static Dictionary<string, string> GetShopList()
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ShopNo,ShopName FROM Dict_ShopInfo");

            var dic = new Dictionary<string, string>();
            foreach (DataRow row in table.Rows)
            {
                dic.Add(row["ShopNo"].ToString(), row["ShopName"].ToString());
            }

            return dic;
        }

        public static List<ShopInfo> GetAreaShopList()
        {
            string sql = "SELECT b.AreaNo,b.AreaName,a.ShopNo,a.ShopName FROM Dict_ShopInfo a INNER JOIN Dict_ShopArea b ON a.AreaNo = b.AreaNo ORDER BY AreaNo,ShopNo";
            var table = SqlHelper.ExecuteDataTable(sql);

            var list = new List<ShopInfo>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new ShopInfo()
                {
                    AreaNo = row["AreaNo"].ToString(),
                    AreaName = row["AreaName"].ToString(),
                    ShopNo = row["ShopNo"].ToString(),
                    ShopName = row["ShopName"].ToString(),
                    IsMapped = false
                });
            }

            return list;
        }

        public static List<ProdInfo> GetProdList()
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ProdNo,ProdName,ProdUnit,CONVERT(FLOAT,Price) AS Price FROM Dict_ProdInfo WHERE State != 1");

            List<ProdInfo> list = new List<ProdInfo>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new ProdInfo()
                {
                    ProdNo = row["ProdNo"].ToString(),
                    ProdName = row["ProdName"].ToString(),
                    ProdUnit = row["ProdUnit"].ToString(),
                    Price = row["Price"].ToString()
                });
            }

            return list;
        }

        public static List<CateProdInfo> GetCateProdList()
        {
            var table = SqlHelper.ExecuteDataTable("SELECT b.PareNo,b.CateNo,b.CateName,a.ProdNo,a.ProdName " +
                "FROM Dict_ProdInfo a " +
                "RIGHT JOIN Dict_ProdCate b ON a.CateNo = b.CateNo WHERE a.State != 1 OR a.State IS NULL");

            var list = new List<CateProdInfo>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new CateProdInfo
                {
                    PareNo = row["PareNo"].ToString(),
                    CateNo = row["CateNo"].ToString(),
                    CateName = row["CateName"].ToString(),
                    ProdNo = row["ProdNo"].ToString(),
                    ProdName = row["ProdName"].ToString()
                });
            }

            return list;
        }

        public static object GetShopInfo(string shopNo)
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ShopNo,ShopName FROM Dict_ShopInfo WHERE ShopNo = '" + shopNo + "'");

            if (table.Rows.Count < 1)
                return null;

            var res = new
            {
                ShopNo = table.Rows[0]["ShopNo"].ToString(),
                ShopName = table.Rows[0]["ShopName"].ToString(),
            };

            return res;
        }

        public static ConnectManag GetConnect(string userId)
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"].ToString();

            //商户信息
            //RouteBusinMeta busin = RouteBusinOpera.GetBusinInfo(userId, secretKey);
            RouteManageOpera oper = new RouteManageOpera(secretKey);
            RouteBusinMeta busin = oper.GetBusinInfo(userId);
            if (busin == null) return null;
            if (busin.AgentProxy == null) return null;

            ConnectFactory factory = new ConnectFactory(sys.separ(busin.AgentProxy));
            factory.SetBusin(busin.BusinNo, busin.BusinPass, "BuyTake");

            var connect = new ConnectManag(factory, new CacheParam("BuyTake"));
            connect.InitSign(SignManag.SystemSign());

            return connect;
        }

        public static TaskMeta NewTask(string sid, string shopNo)
        {
            TaskMeta Task = new TaskMeta();
            Task.Topic = TaskTopic.BuyTake.ToString();
            Task.ShopNo = shopNo;

            Task.OrderSID = sid;
            Task.OrderType = 0;

            return Task;
        }

        public static void SendBakNotice(string userId, string orderId, string shopNo)
        {
            var conn = GetConnect(userId);
            if (conn is null) return;

            TaskNotice Task = conn.GetNoticeTask();
            Task.SendTask(NewTask(orderId, shopNo));
        }

        public static string CreateBakOrder(OrderEntity entity)
        {
            TakeMasterMeta master = new TakeMasterMeta();
            master.BuyState = BuyTakeBuyState.Success;
            master.State = BuyTakeState.Success;
            master.PayType = BuyTakePayType.Paid;
            master.OptTime = entity.OptTime.ToString();
            master.MemoStr = entity.MemoStr;
            master.DeliverFee = entity.DeliverFee;
            master.DeliverAddress = entity.DeliverAddress;
            master.DeliverTime = entity.DeliverTime.ToString();
            master.UserMobile = entity.UserMobile;
            master.UserName = entity.UserName;
            master.Consume = entity.Consume;
            master.TtlPrice = entity.TtlPrice;
            master.OrderType = (BuyTakeOrderType)entity.OrderType;
            master.OrderId = entity.OrderId;
            master.TakeType = (BuyTakeType)entity.TakeType;

            BuyDetailList details = new BuyDetailList();
            foreach (var dtl in entity.OrderDtls)
            {
                details.Add(new BuyDetailMeta
                {
                    ProdName = dtl.ProdName,
                    ProdNo = dtl.ProdNo,
                    ProdUnit = dtl.ProdUnit,
                    Price = dtl.Price,
                    ItemCnt = dtl.ItemCnt,
                    ItemSum = dtl.ItemSum
                });
            }

            var takeBill = new BuyTakeBill(GetConnect(entity.UserId));
            string err;
            takeBill.PreSave(entity.ShopNo, master, details, out err);

            return err;
        }

        [TEJ("", true, Age = 2)]
        public static S TransToMeta<T, S>(T t) where T : class, new() where S : class, new()
        {
            Type tType = typeof(T);
            Type sType = typeof(S);

            foreach (var property in tType.GetProperties())
            {
                var res = property.InvolveAttributeMethod<TEJAttribute>("GetAge",new object[] { 1});

                var attribute = property.GetCustomAttribute<TEJAttribute>();
                if (attribute != null)
                {

                }
            }
            
            return new S();
        }
    }

    public static class AttributeExtention
    {
        public static object InvolveAttributeMethod<T>(this PropertyInfo propertyInfo, string MethodName, object[] parameters) where T : Attribute
        {
            var attribute = propertyInfo.GetCustomAttribute<T>();

            if (attribute != null)
            {
                var meth = attribute.GetType().GetMethod(MethodName);

                var result = meth.Invoke(attribute, parameters);

                return result;
            }

            return null;
        }
    }

    
    public enum TXM
    {
        [Display(Prompt = "")]
        one = 1,
        two = 2
    }

    //[AttributeUsage(AttributeTargets.Property,AllowMultiple = true)]
    public class TEJAttribute : Attribute
    {
        public TEJAttribute(string name)
        {
            Name = name;
        }

        public TEJAttribute(bool butiful)
        {
            Butiful = butiful;
        }

        public TEJAttribute(string name, bool butiful)
        {
            Name = name;
            Butiful = butiful;
        }

        public string Name { get; }
        public bool Butiful { get; set; }
        public int Age { get; set; }
    }

    public class ShopInfo
    {
        public string AreaNo { get; set; }
        public string AreaName { get; set; }
        public string ShopNo { get; set; }
        public string ShopName { get; set; }
        public bool IsMapped { get; set; }
    }

    public class ProdInfo
    {
        public string ProdNo { get; set; }
        public string ProdName { get; set; }
        public string ProdUnit { get; set; }
        public string Price { get; set; }
    }

    public class CateProdInfo
    {
        public string PareNo { get; set; }
        public string CateNo { get; set; }
        public string CateName { get; set; }
        public string ProdNo { get; set; }
        public string ProdName { get; set; }
    }
}
