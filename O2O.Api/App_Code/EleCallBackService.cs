using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.Controllers;
using O2O.Api.Models.Eleme;
using O2O.Common;
using O2O.DTO.Eleme;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using O2O.Service;
using O2O.Service.Eleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace O2O.Api.App_Code
{
    public class EleCallBackService
    {
        private IPreProdService _preProdService { get; set; }
        private IShopConfigService _shopConfigService { get; set; }
        public IEleShopService _shopEleService { get; set; }
        private static ILog _log = LogManager.GetLogger("EleCallBackService");

        public EleCallBackService()
        {
            _preProdService = new PreProdService();
            _shopConfigService = new ShopConfigService();
        }

        public void HandlePushMessage(string res)
        {
            var model = JsonConvert.DeserializeObject<MessageModel>(res);

            if (model.type == 10)
            {
                OrderNew(model);
            }
            else if (model.type >= 12 && model.type <= 18)
            {
                StateChange(model);
            }
            else if (model.type >= 20 && model.type <= 26)
            {
                OrderCancel(model);
            }
            else if (model.type >= 30 && model.type <= 36)
            {
                OrderRefund(model);
            }
            else if (model.type == 45)
            {
                OrderUrge(model);
            }
            else if (model.type >= 51 && model.type <= 56)
            {
                OrderDelivering(model);
            }
        }

        public void OrderNew(MessageModel message)
        {
            IEleShopService shopservice = new EleShopService();
            IOrderService orderService = new OrderService();

            var shop = shopservice.GetByShopId(message.shopId);
            if (shop is null) return;

            var model = JsonConvert.DeserializeObject<OrderModel>(message.message);

            //if (orderService.IsExist(model.id)) return;
            if (orderService.IsExist(model.orderId)) return;


            var entity = new OrderEntity()
            {
                UserId = shop.UserId,
                ShopNo = shop.ShopNo,
                TakeType = 1,
                OrderId = model.orderId ?? model.id,
                TtlPrice = model.originalPrice,
                Consume = model.totalPrice,
                UserName = model.consignee,
                UserMobile = string.Join(",", model.phoneList),
                DeliverTime = model.deliverTime == null ? new DateTime(1970, 1, 1) : DateTime.Parse(model.deliverTime.ToString()),
                DeliverAddress = model.address,
                DeliverFee = model.deliverFee,
                MemoStr = model.description,
                OptTime = model.activeAt,
                //PayType = 0,
                State = model.status.GetOrder(),
                DaySeq = model.daySn,
                Greeting = model.userExtraInfo.greeting  //祝福语（饿了么）
            };

            var list = new List<OrderDtlEntity>();
            foreach (var group in model.groups)
            {
                if (group.type != "normal") continue; //type:normal extra ，排除其他
                foreach (var item in group.items)
                {
                    var prodNo = item.extendCode ?? "";

                    //如果商品未映射，且价格为0，则忽略,排除广告
                    if (string.IsNullOrWhiteSpace(prodNo) && item.price == 0)
                    {
                        continue;
                    }

                    var prodCountList = prodNo.Trim('X').Split('X').GroupBy(a => a).Select(a => new { ProdNo = a.Key, Count = a.Count() });

                    foreach (var prodCount in prodCountList)
                    {
                        //list.Add(new OrderDtlEntity()
                        //{
                        //    ProdNo = prodCount.ProdNo,
                        //    ProdName = item.name,
                        //    ProdUnit = "",
                        //    Price = item.price / prodCount.Count,
                        //    ItemCnt = item.quantity * prodCount.Count,
                        //    ItemSum = item.price * item.quantity
                        //});

                        //改为取365商品信息，如果未匹配上365商品，则取饿了么商品信息
                        var prod = Bak365Service.GetProd(entity.UserId, prodCount.ProdNo);

                        list.Add(new OrderDtlEntity()
                        {
                            ProdNo = prod.ProdNo ?? "",
                            ProdName = string.IsNullOrWhiteSpace(prod.ProdName) ? item.name : prod.ProdName + item.attributes?.Aggregate(" ", (x, y) => x + (y.value+",")).TrimEnd(','),  //咖啡 热,不需要
                            ProdUnit = prod.ProdUnit ?? "",
                            Price = double.Parse(prod.Price ?? item.price.ToString()),
                            ItemCnt = item.quantity * prodCount.Count,
                            ItemSum = double.Parse(prod.Price ?? item.price.ToString()) * item.quantity * prodCount.Count
                        });
                    }
                }
            }
            entity.OrderDtls = list;

            try
            {
                //判断商品中是否有预订商品
                if (_preProdService.hasPreProd(shop.UserId, list.Select(a => a.ProdNo).ToArray()))
                {
                    entity.OrderType = 1;
                    entity.BuyState = 2; //0未生成现购单 1已生成现购单 2已生成预定单 3已提货 4已预定作废

                    Bak365Service.CreateBakOrder(entity, "Pre");

                    ////处理远程创建订单死锁的问题，循环5次
                    //var times = 0;
                    //int[] seconds = { 2000, 5000, 10000, 30000, 60000 };
                    //while (times < 5)
                    //{
                    //     Bak365Service.CreateBakOrder(entity, "Pre");
                    //    //如果生成预订单成功，则更改生成状态
                    //    if (string.IsNullOrWhiteSpace(""))
                    //    {
                            
                    //        break;
                    //    }

                    //    await Task.Delay(seconds[times]);
                    //    times++;
                    //}
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:CreateOrder 信息:{0} 错误:{1}", JsonConvert.SerializeObject(model), e.Message);
            }
            finally
            {
                orderService.Add(entity);
                //发送通知
                Bak365Service.SendBakNotice(entity.UserId, entity.OrderId, entity.ShopNo, 0);
            }

            //是否自动接单
            //var shopConfig = _shopConfigService.GetAsync(shop.UserId, shop.ShopNo).Result;
            //if (shopConfig.EleAutoConfirm == 0)
            //{
            //    //var controller = new OrderController();
            //    //controller.Confirm(shop.UserId, shop.ShopNo, 1, model.orderId);
            //    var service = new EleOrderApiService();
            //    var res = service.ConfirmOrderLite(shop.AccessToken, model.orderId);
            //}
        }

        public void StateChange(MessageModel message) {
            IOrderService orderService = new OrderService();

            var model = JsonConvert.DeserializeObject<StateChangeModel>(message.message);

            var entity = orderService.GetByOrderId(model.orderId);
            if (entity == null) return;
            entity.State = model.state.GetOrder();

            orderService.Update(entity);
        }

        public void OrderCancel(MessageModel message)
        {
            IOrderService orderService = new OrderService();
            var cancelModel = JsonConvert.DeserializeObject<CancelModel>(message.message);
            var byShopId = new EleShopService().GetByShopId(cancelModel.shopId);
            if (byShopId == null) return;
            
            Bak365Service.SendBakNotice(byShopId.UserId, cancelModel.orderId, byShopId.ShopNo, 1);
            var order = orderService.GetByOrderId(cancelModel.orderId);

            if (order == null) return;

            order.State = cancelModel.refundStatus.GetOrder();
            order.CancelCode = cancelModel.refundStatus.ToString();
            order.CancelReason = cancelModel.reason;
            orderService.Update(order);
        }

        public void OrderRefund(MessageModel message)
        {
            IOrderService orderService = new OrderService();
            var cancelModel = JsonConvert.DeserializeObject<CancelModel>(message.message);
            var byShopId = new EleShopService().GetByShopId(cancelModel.shopId);

            if (byShopId == null) return;

            Bak365Service.SendBakNotice(byShopId.UserId, cancelModel.orderId, byShopId.ShopNo, 1);
            var byOrderId = orderService.GetByOrderId(cancelModel.orderId);
            if (byOrderId == null) return;

            byOrderId.State = cancelModel.refundStatus.GetOrder();
            byOrderId.RefundCode = cancelModel.refundStatus.ToString();
            byOrderId.RefundReason = cancelModel.reason;

            if (cancelModel.refundType == "part" && cancelModel.refundStatus == ElemeEnum.Refund.successful)
            {
                byOrderId.State = 4;
                byOrderId.RefundPartAmt = cancelModel.totalPrice;
                foreach (var goods in cancelModel.goodsList)
                {
                    var good = goods;
                    var orderDtlEntity = byOrderId.OrderDtls.FirstOrDefault(a => a.ProdName == good.name);
                    if (orderDtlEntity != null)
                        orderDtlEntity.RefundPartCnt = good.quantity;
                }
            }
            orderService.Update(byOrderId);
            orderService.UpdateDtl(byOrderId.OrderDtls);
        }

        public void OrderUrge(MessageModel message)
        {
            
        }

        public void OrderDelivering(MessageModel message)
        {
            IOrderService orderService = new OrderService();

            var model = JsonConvert.DeserializeObject<Delivering>(message.message);

            var entity = orderService.GetByOrderId(model.orderId);
            if (entity == null) return;

            if (string.IsNullOrWhiteSpace(model.phone)) return;
            entity.DispatcherName = model.name;
            entity.DispatcherMobile = model.phone;

            //骑手已经取餐&开启自动生成销售单
            if (message.type == 55)
            {
                var shopConfig = _shopConfigService.GetAsync(entity.UserId, entity.ShopNo).Result;
                if (shopConfig.AutoSale == 1)
                {
                    if (entity.OrderType == 0) //现购
                    {
                        Bak365Service.CreateBakOrder(entity, "Sale");
                        entity.BuyState = 1;
                    }
                    else if (entity.OrderType == 1) //预定
                    {
                        Bak365Service.FinPreOrder(entity.UserId, entity.OrderId);
                        entity.BuyState = 3;
                    }
                }
            }

            orderService.Update(entity);
        }
    }
}