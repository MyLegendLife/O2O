using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;

namespace O2O.Service
{
    public class OrderService : IOrderService
    {
        public void Add(OrderEntity entity)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                service.Add(entity);
            }
        }

        public void Update(OrderEntity entity)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                service.Update(entity);
            }
        }

        public void UpdateEntityFields(OrderEntity entity, List<string> fileds)
        {
            using (var db = new O2OContext())
                new BaseService<OrderEntity>(db).UpdateEntityFields(entity, fileds);
        }

        public void UpdateDtl(IEnumerable<OrderDtlEntity> entities)
        {
            using (var db = new O2OContext())
            {
                var baseService = new BaseService<OrderDtlEntity>(db);
                foreach (var entity in entities)
                    baseService.UpdateForce(entity);
            }
        }

        public void Update(OrderEntity entity, Dictionary<string, object> dic)
        {
            ToolsCommon.SetValue(entity, dic);

            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                service.Update(entity);
            }
        }

        public void UpdateState(string orderId, int state)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);
                var entity = service.Entities.FirstOrDefault(a => a.OrderId == orderId);
                entity.State = state;

                service.Update(entity);
            }
        }

        public bool IsExist(string orderId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                var count = service.Count(a => a.OrderId == orderId);

                return count != 0;
            }
        }

        public void SetBuy(string userId, int buyState)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                var entity = service.FirstOrDefault(a => a.OrderId == userId);
                entity.BuyState = buyState;

                service.Update(entity);
            }
        }

        public OrderEntity Get(string id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                return service.FirstOrDefault(a => a.Id.ToString() == id);
            }
        }

        public OrderEntity Get(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                return service.FirstOrDefault(a => a.Id == id);
            }
        }

        public int GetCount(string userId)
        {
            using (var db = new O2OContext())
                return new BaseService<OrderEntity>(db).Count(a => a.UserId == userId);
        }

        public OrderEntity GetByOrderId(string orderId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                var entity = service.Where(a => a.OrderId == orderId).Include(a => a.OrderDtls).FirstOrDefault();
                return entity;
            }
        }

        //public OrderInfoDTO GetInfoByOrderId(string orderId)
        //{
        //    using (O2OContext context = new O2OContext())
        //    {
        //        BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

        //        var entity = service.Where(a => a.OrderId == orderId).Include(a => a.OrderDtls).FirstOrDefault();

        //        var dto = ToolsCommon.EntityToEntity(entity, new OrderInfoDTO()) as OrderInfoDTO;

        //        if (dto != null && dto.DeliverTime.Contains("1970"))
        //        {
        //            dto.DeliverTime = "立即送达";
        //        }
        //        return dto;
        //    }
        //}

        public List<OrderDTO> GetOrders(string userId, string shopNo, JObject query)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                Expression<Func<OrderEntity, bool>> where = a => a.UserId == userId && a.ShopNo == shopNo;

                if (query["TakeType"] != null && query["TakeType"].ToString() != "")
                {
                    var x = query["TakeType"].ToString();
                    where = where.And(a => a.TakeType.ToString() == x);
                }
                if (query["SearchKay"] != null && query["SearchKay"].ToString() != "")
                {
                    var x = query["SearchKay"].ToString();
                    where = where.And(a => a.OrderId.Contains(x) || a.UserName.Contains(x) || a.DaySeq.ToString() == x);
                }
                //if (query["OrderId"] != null)
                //{
                //    var x = query["OrderId"].ToString();
                //    where = where.And(a => a.OrderId.Contains(x));
                //}
                //if (query["UserName"] != null)
                //{
                //    var x = query["UserName"].ToString();
                //    where = where.And(a => a.UserName.Contains(x));
                //}
                //if (query["UserMobile"] != null)
                //{
                //    var x = query["UserMobile"].ToString();
                //    where = where.And(a => a.UserMobile.Contains(x));
                //}
                //if (query["PayType"] != null)
                //{
                //    var x = query["PayType"].ToString();
                //    where = where.And(a => a.PayType.ToString() == x);
                //}
                if (query["State"] != null && query["State"].ToString() != "")
                {
                    var x = query["State"].ToString();
                    //where = where.And(a => a.State.ToString() == x);
                    where = where.And(a => x.Contains((a.State.ToString())));
                }
                if (query["OptTimeSta"] != null)
                {
                    var x = DateTime.Parse(query["OptTimeSta"].ToString());
                    where = where.And(a => a.OptTime >= x);
                }
                if (query["OptTimeEnd"] != null)
                {
                    var x = DateTime.Parse(query["OptTimeEnd"] + " 23:59:59");
                    where = where.And(a => a.OptTime <= x);
                }

                var list = service.Entities.Where(where)
                    .ToList()
                    .Select(a => ToolsCommon.EntityToEntity(a, new OrderDTO()) as OrderDTO)
                    .ToList()
                    .OrderByDescending(a => a.OptTime).ToList();

                //foreach (var item in list)
                //{
                //    if (item != null && item.DeliverTime.Contains("1970"))
                //    {
                //        item.DeliverTime = "立即送达";
                //    }
                //}

                return list;
            }
        }

        public List<OrderDtlDTO> GetOrderDtls(string orderId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<OrderEntity>(context);

                var entity = service.Entities.FirstOrDefault(a => a.OrderId == orderId);

                var list = new List<OrderDtlDTO>();
                if (entity != null)
                {
                    var dtls = entity.OrderDtls.ToList();

                    foreach (var dtl in dtls)
                    {
                        var prodCountList = dtl.ProdNo.Trim('X').Split('X').
                                GroupBy(a => a).
                                Select(b => new
                                {
                                    ProdNo = b.Key,
                                    Count = b.Count()
                                });

                        foreach (var prodCount in prodCountList)
                        {
                            list.Add(new OrderDtlDTO()
                            {
                                ProdNo = prodCount.ProdNo,
                                ProdName = dtl.ProdName,
                                ProdUnit = dtl.ProdUnit,
                                Price = dtl.Price / prodCount.Count,
                                ItemCnt = dtl.ItemCnt * prodCount.Count,
                                ItemSum = dtl.ItemSum,
                                RefundPartCnt = dtl.RefundPartCnt * prodCount.Count
                            });
                        }
                    }

                    //list = entity.OrderDtls.ToList().Select(a => ToolsCommon.EntityToEntity(a, new OrderDtlDTO()) as OrderDtlDTO).ToList();
                }

                return list;
            }
        }

        public int[] GetMissOrder(string userId, DateTime dateTime, string shopNo, int takeType)
        {
            Expression<Func<OrderEntity, bool>> expression = a => a.UserId.Equals(userId) && SqlFunctions.DateDiff("d", dateTime, a.OptTime) == 0 && a.ShopNo.Equals(shopNo) && a.TakeType.Equals(takeType);
            if (takeType == 1)
                expression = a => a.UserId.Equals(userId) && (SqlFunctions.DateDiff("d", dateTime, a.OptTime) == (int?)0 && SqlFunctions.DateDiff("d", a.DeliverTime, "1970-01-01") == 0 || SqlFunctions.DateDiff("d", dateTime, a.DeliverTime) == 0) && a.ShopNo.Equals(shopNo) && a.TakeType.Equals(takeType);
            using (var db = new O2OContext())
            {
                var baseService = new BaseService<OrderEntity>(db);

                var data = db.Order.Where(expression).GroupBy(a => new
                {
                    ShopNo = a.ShopNo
                }).Select(g => new
                {
                    MaxDaySeq = g.Max(a => a.DaySeq),
                    Count = g.Count()
                }).FirstOrDefault();
                var numArray = new int[0];
                if (data == null || data.MaxDaySeq == data.Count)
                    return numArray;
                var arrayDaySeq = baseService.Where(expression).Select(a => a.DaySeq).ToArray();
                numArray = Enumerable.Range(1, data.MaxDaySeq).Where(a => !arrayDaySeq.Contains(a)).ToArray();
                return numArray;
            }
        }

        public Dictionary<string, int[]> GetMissOrder(string userId, DateTime dateTime, int takeType)
        {
            Expression<Func<OrderEntity, bool>> exp = a => SqlFunctions.DateDiff("d", dateTime, a.OptTime) == 0;


            using (var context = new O2OContext())
            {
                var dfd = from a in context.Order
                          where a.UserId == userId && exp.Compile()(a)
                          && SqlFunctions.DateDiff("d", dateTime, a.OptTime) == 0
                          group a by a.ShopNo into g
                          select new
                          {
                              MaxDaySeq = g.Max(a => a.DaySeq),
                              Count = g.Count()
                          };

                var dict = new Dictionary<string, int[]>();


            }

            //Expression<Func<OrderEntity, bool>> expression = a => a.UserId.Equals(userId) && SqlFunctions.DateDiff("d", (DateTime?)dateTime, (DateTime?)a.OptTime) == 0 && a.ShopNo.Equals(shopNo) && a.TakeType.Equals(takeType);
            //if (takeType == 1)
            //    expression = a => a.UserId.Equals(userId) && (SqlFunctions.DateDiff("d", (DateTime?)dateTime, (DateTime?)a.OptTime) == (int?)0 && SqlFunctions.DateDiff("d", (DateTime?)a.DeliverTime, "1970-01-01") == 0 || SqlFunctions.DateDiff("d", (DateTime?)dateTime, (DateTime?)a.DeliverTime) == 0) && a.ShopNo.Equals(shopNo) && a.TakeType.Equals(takeType);
            //using (O2OContext db = new O2OContext())
            //{
            //    BaseService<OrderEntity> baseService = new BaseService<OrderEntity>(db);

            //    var data = db.Order.Where(expression).GroupBy(a => new
            //    {
            //        ShopNo = a.ShopNo
            //    }).Select(g => new
            //    {
            //        MaxDaySeq = g.Max(a => a.DaySeq),
            //        Count = g.Count()
            //    }).FirstOrDefault();
            //    int[] numArray = new int[0];
            //    if (data == null || data.MaxDaySeq == data.Count)
            //        return numArray;
            //    int[] arrayDaySeq = baseService.Where(expression).Select(a => a.DaySeq).ToArray();
            //    numArray = Enumerable.Range(1, data.MaxDaySeq).Where(a => !((IEnumerable<int>)arrayDaySeq).Contains(a)).ToArray();
            //    return numArray;
            //}

            return null;
        }

        public List<OrderDtlGroupDto> GetOrderProdList(string userId, string text)
        {
            using (var context = new O2OContext())
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    var list = from a in context.Order
                        join b in context.OrderDtl on a.Id equals b.OrderId
                        where
                        a.UserId == userId && b.ProdNo == ""
                        group b by new { b.ProdNo, b.ProdName, b.Price } into g
                        select new OrderDtlGroupDto
                        {
                            ProdNo = g.Key.ProdNo,
                            ProdName = g.Key.ProdName,
                            ProdUnit = g.Max(a => a.ProdUnit),
                            Price = g.Key.Price
                        };

                    return list.ToList();
                }
                else
                {
                    var list = from a in context.Order
                        join b in context.OrderDtl on a.Id equals b.OrderId
                        where 
                        a.UserId == userId
                        && (b.ProdNo == text || b.ProdName == text)
                        group b by new { b.ProdNo, b.ProdName, b.Price } into g
                        select new OrderDtlGroupDto
                        {
                            ProdNo = g.Key.ProdNo,
                            ProdName = g.Key.ProdName,
                            ProdUnit = g.Max(a => a.ProdUnit),
                            Price = g.Key.Price
                        };

                    return list.ToList();
                }
            }
        }

        public void UpdateProdNoMapInfo(
          string userId,
          string prodNo,
          string prodName,
          string prodNoNew)
        {
            using (var o2Ocontext = new O2OContext())
            {
                var sql = "WITH t AS (SELECT b.* FROM T_Order a INNER JOIN T_OrderDtl b ON a.Id = b.OrderId WHERE a.UserId= '" + userId + "' AND b.ProdNo = '" + prodNo + "' AND b.ProdName = '" + prodName + "') UPDATE t SET ProdNo = '" + prodNoNew + "'";
                o2Ocontext.Database.ExecuteSqlCommand(sql);
            }
        }

        public void DeleteProdNoMapInfo(string userId, string prodNo, string prodName)
        {
            using (var o2Ocontext = new O2OContext())
            {
                var sql = "WITH t AS (SELECT b.* FROM T_Order a INNER JOIN T_OrderDtl b ON a.Id = b.OrderId WHERE a.UserId= '" + userId + "' AND b.ProdNo = '" + prodNo + "' AND b.ProdName = '" + prodName + "') DELETE T_OrderDtl WHERE Id IN (SELECT Id FROM t)";
                o2Ocontext.Database.ExecuteSqlCommand(sql);
            }
        }
    }
}
