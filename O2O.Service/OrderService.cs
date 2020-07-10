using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace O2O.Service
{
    public class OrderService : IOrderService
    {
        public void Add(OrderEntity entity)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                service.Add(entity);
                NoticeCommon.Notice(entity.UserId, entity.OrderId, entity.ShopNo);
            }
        }

        public void Update(OrderEntity entity)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                service.Update(entity);
            }
        }

        public void Update(OrderEntity entity, Dictionary<string, object> dic)
        {
            ToolsCommon.SetValue(entity,dic);

            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                service.Update(entity);
            }
        }

        public void UpdateState(string orderId, int state)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);
                var entity = service.Entities.FirstOrDefault(a => a.OrderId == orderId);
                entity.State = state;

                service.Update(entity);
            }
        }

        public bool IsExist(string orderId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                int count = service.Count(a => a.OrderId == orderId);

                return count != 0;
            }
        }

        public void SetBuy(string userId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                OrderEntity entity = service.FirstOrDefault(a => a.OrderId == userId);
                entity.BuyState = 1;

                service.Update(entity);
            }
        }

        public OrderEntity Get(string id)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                return service.FirstOrDefault(a => a.Id.ToString() == id);
            }
        }

        public OrderEntity Get(Guid id)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                return service.FirstOrDefault(a => a.Id == id);
            }
        }

        public OrderEntity GetByOrderId(string orderId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                return service.FirstOrDefault(a => a.OrderId == orderId);
            }
        }

        public List<OrderDTO> GetOrders(string userId, string shopNo, JObject query)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                Expression<Func<OrderEntity, bool>> where = a => a.UserId == userId && a.ShopNo== shopNo;

                if (query["TakeType"] != null)
                {
                    var x = query["TakeType"].ToString();
                    where = where.And(a => a.TakeType.ToString() == x);
                }
                if (query["OrderId"] != null)
                {
                    var x = query["OrderId"].ToString();
                    where = where.And(a => a.OrderId.Contains(x));
                }
                if (query["UserName"] != null)
                {
                    var x = query["UserName"].ToString();
                    where = where.And(a => a.UserName.Contains(x));
                }
                if (query["UserMobile"] != null)
                {
                    var x = query["UserMobile"].ToString();
                    where = where.And(a => a.UserMobile.Contains(x));
                }
                if (query["PayType"] != null)
                {
                    var x = query["PayType"].ToString();
                    where = where.And(a => a.PayType.ToString() == x);
                }
                if (query["State"] != null)
                {
                    var x = query["State"].ToString();
                    where = where.And(a => a.State.ToString() == x);
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
                    .Select(a => ToolsCommon.EntityToEntity(a,new OrderDTO()) as OrderDTO)
                    .ToList()
                    .OrderByDescending(a => a.OptTime).ToList();

                return list;
            }
        }

        public List<OrderDtlDTO> GetOrderDtls(string orderId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<OrderEntity> service = new BaseService<OrderEntity>(context);

                var entity = service.Entities.FirstOrDefault(a => a.OrderId == orderId);

                var list = new List<OrderDtlDTO>();
                if (entity != null)
                {
                    var dtls = entity.OrderDtls.ToList();

                    foreach (var dtl in dtls)
                    {
                        foreach (var prodNo in dtl.ProdNo.Split('X'))
                        {
                            list.Add(new OrderDtlDTO()
                            {
                                ProdNo = prodNo,
                                ProdName = dtl.ProdName,
                                ProdUnit = dtl.ProdUnit,
                                Price = dtl.Price,
                                ItemCnt = dtl.ItemCnt,
                                ItemSum = dtl.ItemSum
                            });
                        }
                    }

                    //list = entity.OrderDtls.ToList().Select(a => ToolsCommon.EntityToEntity(a, new OrderDtlDTO()) as OrderDtlDTO).ToList();
                }

                return list;
            }
        }
    }
}
