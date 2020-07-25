using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.Models.Eleme;
using O2O.Common;
using O2O.IService;
using O2O.Model;
using O2O.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.Api.App_Code
{
    public class EleCallBackService
    {
        private IPreProdService _preProdService { get; set; }

        public EleCallBackService()
        {
            _preProdService = new PreProdService();
        }

        public void HandlePushMessage(string res)
        {
            MessageModel model = JsonConvert.DeserializeObject<MessageModel>(res);

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
        }

        public void OrderNew(MessageModel message)
        {
            IEleShopService shopservice = new EleShopService();
            IOrderService orderService = new OrderService();

            var shop = shopservice.GetByShopId(message.shopId);
            if (shop is null) return;

            OrderModel model = JsonConvert.DeserializeObject<OrderModel>(message.message);

            if (orderService.IsExist(model.orderId)) return;

            var entity = new OrderEntity()
            {
                UserId = shop.UserId,
                ShopNo = shop.ShopNo,
                TakeType = 1,
                OrderId = model.orderId,
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
                DaySeq = model.daySn
            };

            var list = new List<OrderDtlEntity>();
            foreach (var group in model.groups)
            {
                foreach (var item in group.items)
                {
                    string prodNo = item.extendCode ?? "";
                    list.Add(new OrderDtlEntity()
                    {
                        ProdNo = prodNo,
                        ProdName = item.name,
                        ProdUnit = "",
                        Price = item.price,
                        ItemCnt = item.quantity,
                        ItemSum = item.price * item.quantity
                    });
                }
            }
            entity.OrderDtls = list;

            //发送通知
            Bak365Service.SendBakNotice(entity.UserId, entity.OrderId, entity.ShopNo);
            //判断商品中是否有预订商品
            if (_preProdService.hasPreProd(shop.UserId, list.Select(a => a.ProdNo).ToArray()))
            {
                entity.OrderType = 1;
                string err = Bak365Service.CreateBakOrder(entity);
                //如果生成预订单成功，则更改生成状态
                if (string.IsNullOrWhiteSpace(err))
                {
                    entity.BuyState = 1;
                }
            }
            
            orderService.Add(entity);
        }

        public void StateChange(MessageModel message) {
            IOrderService orderService = new OrderService();

            StateChangeModel model = JsonConvert.DeserializeObject<StateChangeModel>(message.message);

            var entity = orderService.GetByOrderId(model.orderId);
            if (entity == null) return;
            entity.State = model.state.GetOrder();

            orderService.Update(entity);
        }

        public void OrderCancel(MessageModel message)
        {
            IOrderService orderService = new OrderService();

            CancelModel model = JsonConvert.DeserializeObject<CancelModel>(message.message);

            var entity = orderService.GetByOrderId(model.orderId);
            if (entity == null) return;
            entity.State = model.refundStatus.GetOrder();
            entity.CancelCode = model.refundStatus.ToString();
            entity.CancelReason = model.reason;

            orderService.Update(entity);
        }

        public void OrderRefund(MessageModel message)
        {
            IOrderService orderService = new OrderService();

            CancelModel model = JsonConvert.DeserializeObject<CancelModel>(message.message);

            var entity = orderService.GetByOrderId(model.orderId);
            if (entity == null) return;
            entity.State = model.refundStatus.GetOrder();
            entity.RefundCode = model.refundStatus.ToString();
            entity.RefundReason = model.reason;

            orderService.Update(entity);
        }

        public void OrderUrge(MessageModel message)
        {
            
        }
    }
}