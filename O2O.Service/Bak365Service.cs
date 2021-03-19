using ModuBase;
using ModuData;
using ModuHandleBase;
using ModuHandleBuy;
using ModuInterBuy;
using ModuRoute;
using O2O.Common;
using O2O.Model.Entities;
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
            var sql = "SELECT b.AreaNo,b.AreaName,a.ShopNo,a.ShopName FROM Dict_ShopInfo a INNER JOIN Dict_ShopArea b ON a.AreaNo = b.AreaNo ORDER BY AreaNo,ShopNo";
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

        public static object GetProdInfo(string prodNo)
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ProdNo,ProdName FROM Dict_ProdInfo WHERE ProdNo = '" + prodNo + "'");

            if (table.Rows.Count < 1)
                return null;

            var res = new
            {
                ProdNo = table.Rows[0]["ProdNo"].ToString(),
                ProdName = table.Rows[0]["ProdName"].ToString(),
            };

            return res;
        }

        public static List<ProdInfo> GetProdList()
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ProdNo,ProdName,ProdUnit,CONVERT(FLOAT,Price) AS Price FROM Dict_ProdInfo WHERE State != 1");

            var list = new List<ProdInfo>();
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

        public static ProdInfo GetProd(string userId,string prodNo)
        {
            var service = new UserService();
            var user = service.Get(userId);

            var table = SqlHelper.ExecuteDataTable($"SELECT TOP 1 ProdNo,ProdName,ProdUnit,CONVERT(FLOAT,Price) AS Price FROM Dict_ProdInfo WHERE State != 1 AND ProdNo = '{prodNo}'",
                ToolsCommon.FromBase64(user.ConnString));

            if (table.Rows.Count == 0) return new ProdInfo();
           
            return new ProdInfo()
            {
                ProdNo = table.Rows[0]["ProdNo"].ToString(),
                ProdName = table.Rows[0]["ProdName"].ToString(),
                ProdUnit = table.Rows[0]["ProdUnit"].ToString(),
                Price = table.Rows[0]["Price"].ToString()
            };
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

        //是否存现购单和预订单
        public static bool IsExistOrder(string userId, string orderId)
        {
            //if (string.IsNullOrWhiteSpace(Global.CON_STR))
            //{
            var service = new UserService();
            var user = service.Get(userId);
            //Global.CON_STR = ToolsCommon.FromBase64(user.ConnString);
            //}

            var sql = $" SELECT SUM(COUNT) AS Count FROM (" +
                      $" SELECT COUNT(ExchNo) AS Count FROM MD_BuySaleConsume Where ExchNo = '{orderId}'" +
                      $" UNION ALL" +
                      $" SELECT COUNT(ExchNo) AS Count FROM MD_BuyPreConsume Where ExchNo = '{orderId}'" +
                      $" ) X";
            var table = SqlHelper.ExecuteDataTable(sql, ToolsCommon.FromBase64(user.ConnString));

            return table.Rows[0]["Count"].ToString() != "0";
        }

        //是否已经提货
        public static bool IsFined(string userId, string orderId)
        {
            var service = new UserService();
            var user = service.Get(userId);

            var sql = $"SELECT State FROM MD_BuyPreConsume WHERE ExchNo = '{orderId}'";

            var table = SqlHelper.ExecuteDataTable(sql, ToolsCommon.FromBase64(user.ConnString));

            //如果没有预订单，则返回true
            if (table.Rows.Count == 0) return true;
            return table.Rows[0]["State"].ToString() != "0";
        }

        public static ConnectManag GetConnect(string userId)
        {
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];

            //商户信息
            //RouteBusinMeta busin = RouteBusinOpera.GetBusinInfo(userId, secretKey);
            var oper = new RouteManageOpera(secretKey);
            var busin = oper.GetBusinInfo(userId);
            if (busin == null) return null;
            if (busin.AgentProxy == null) return null;

            var factory = new ConnectFactory(sys.separ(busin.AgentProxy));
            factory.SetBusin(busin.BusinNo, busin.BusinPass, "BuyTake");

            var connect = new ConnectManag(factory, new CacheParam("BuyTake"));
            connect.InitSign(SignManag.SystemSign());

            return connect;
        }

        public static TaskMeta NewTask(string sid, string shopNo, int orderType)
        {
            var Task = new TaskMeta();
            Task.Topic = TaskTopic.BuyTake.ToString();
            Task.ShopNo = shopNo;

            Task.OrderSID = sid;
            Task.OrderType = orderType;

            return Task;
        }

        public static void SendBakNotice(string userId, string orderId, string shopNo, int orderType = 0)
        {
            var conn = GetConnect(userId);
            if (conn is null) return;

            var Task = conn.GetNoticeTask();
            Task.SendTask(NewTask(orderId, shopNo, orderType));
            conn.Close();
        }

        public static void CreateBakOrder(OrderEntity entity, string type)
        {
            if (IsExistOrder(entity.UserId, entity.OrderId))
            {
                return;
            }

            #region

            var master = new TakeMasterMeta();
            master.BuyState = type == "Sale" ? BuyTakeBuyState.Sale : BuyTakeBuyState.Pre;
            master.State = BuyTakeState.Success;
            master.PayType = BuyTakePayType.Paid;
            master.OptTime = entity.OptTime.ToString();
            master.MemoStr = entity.MemoStr;
            master.DeliverFee = entity.DeliverFee;
            master.DeliverAddress = entity.DeliverAddress;
            master.DeliverTime = entity.DeliverTime > entity.OptTime ? entity.DeliverTime.ToString() : entity.OptTime.ToString();
            master.UserMobile = entity.UserMobile;
            master.UserName = entity.UserName;
            master.Consume = entity.Consume;
            master.TtlPrice = entity.TtlPrice;
            master.OrderType = type == "Sale" ? BuyTakeOrderType.Sale : BuyTakeOrderType.Pre;
            master.OrderId = entity.OrderId;
            master.TakeType = (BuyTakeType)entity.TakeType;

            var details = new BuyDetailList();
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

            #endregion

            //处理远程创建订单死锁的问题，循环5次
            //var times = 0;
            //int[] seconds = { 2000, 5000, 10000, 30000, 60000 };
            //while (times < 5)
            //{

            try
            {
                var conn = GetConnect(entity.UserId);
                if (conn is null) return;

                var takeBill = new BuyTakeBill(conn);
                string err;
                if (type == "Sale")
                {
                    takeBill.SaleSave(entity.ShopNo, master, details, out err);
                }
                else
                {
                    takeBill.PreSave(entity.ShopNo, master, details, out err);
                }

                conn.Close();
            }
            catch (Exception e)
            {
                // ignored
            }

            //await Task.Delay(seconds[times]);
            //Thread.Sleep(seconds[times]);

            //times++;
        }

        public static string FinPreOrder(string userId, string orderId)
        {
            //判断是否已经提货
            if (IsFined(userId, orderId)) return "订单已经提货";

            var conn = GetConnect(userId);
            if (conn is null) return "网络异常";

            var takeBill = new BuyTakeBill(conn);
            string err;

            takeBill.PreFin(orderId, out err);

            conn.Close();
            return err;
        }

        [TEJ("", true, Age = 2)]
        public static S TransToMeta<T, S>(T t) where T : class, new() where S : class, new()
        {
            var tType = typeof(T);
            var sType = typeof(S);

            foreach (var property in tType.GetProperties())
            {
                var res = property.InvolveAttributeMethod<TEJAttribute>("GetAge", new object[] { 1 });

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
