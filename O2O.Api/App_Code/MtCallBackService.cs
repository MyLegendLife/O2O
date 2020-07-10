using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.Models.Meituan;
using O2O.Common;
using O2O.IService;
using O2O.Model;
using O2O.Service;
using System;
using System.Collections.Generic;

namespace O2O.Api.App_Code
{
    public class MtCallBackService
    {
        IOrderService _orderService { get; set; }

        public MtCallBackService()
        {
            _orderService = new OrderService();
        }

        public void Handle(string type,string res,string userId)
        {
            if (string.IsNullOrEmpty(res)) return;

            bool isSpecial = false;
            if (type == "OrderCanceled" || type == "OrderRefunded" || type == "OrderRefundedPart")
            {
                isSpecial = true;
            }

             var obj = StringAnaly(res, isSpecial);
            if (obj.Count <= 1) return; //处理心跳

            Dictionary<string, object> dic = new Dictionary<string, object>();
            switch (type)
            {
                case "OrderConfirmed":
                    dic.Add("State", 1);
                    break;
                case "OrderFinished":
                    dic.Add("State", 4);
                    break;
                case "OrderCanceled":
                    dic.Add("CancelCode", obj["reason_code"].ToString());
                    dic.Add("CancelReason", obj["reason"].ToString());
                    dic.Add("State", 2);
                    break;
                case "OrderRefunded":
                    dic.Add("RefundCode", obj["notify_type"].ToString());
                    dic.Add("RefundReason", obj["reason"].ToString());
                    var enum1 = (MeituanEnum.Refund)Enum.Parse(typeof(MeituanEnum.Refund), obj["notify_type"].ToString());
                    dic.Add("State", enum1.GetOrder());
                    break;
                case "OrderRefundedPart":
                    dic.Add("RefundCode", obj["notify_type"].ToString());
                    dic.Add("RefundReason", obj["reason"].ToString());
                    var enum2 = (MeituanEnum.Refund)Enum.Parse(typeof(MeituanEnum.Refund), obj["notify_type"].ToString());
                    dic.Add("State", enum2.GetOrder());
                    break;
                case "OrderUrge":
                    break;
                case "OrderDeliveringStatus":
                    break;
                case "OrderPrivacy":
                    break;
            }

            OrderUpdate(obj["order_id"].ToString(),dic);
        }

        public void OrderNew(string userId, string res)
        {
            var obj = StringAnaly(res);

            if (obj == null) return;

            var model = JsonConvert.DeserializeObject<OrderModel>(obj.ToString());

            if (_orderService.IsExist(model.order_id)) return;

            var entity = new OrderEntity()
            {
                UserId = userId,
                ShopNo = model.app_poi_code,
                TakeType = 0,
                OrderId = model.order_id,
                TtlPrice = model.original_price,
                Consume = model.total,
                UserName = model.recipient_name,
                UserMobile = model.recipient_phone,
                DeliverTime = model.delivery_time.ToDateTime(),
                DeliverAddress = model.recipient_address,
                DeliverFee = model.shipping_fee,
                MemoStr = model.caution,
                OptTime = model.ctime.ToDateTime(),
                PayType = model.pay_type,
                State = ((MeituanEnum.State)model.status).GetOrder(),
                DaySeq = model.day_seq
            };

            var dtls = JsonConvert.DeserializeObject<List<DetailModel>>(model.detail);

            var list = new List<OrderDtlEntity>();
            foreach (var dtl in dtls)
            {
                list.Add(new OrderDtlEntity()
                {
                    ProdNo = dtl.sku_id,
                    ProdName = dtl.food_name,
                    ProdUnit = dtl.unit,
                    Price = dtl.price,
                    ItemCnt = dtl.quantity,
                    ItemSum = dtl.price * dtl.quantity
                });
            }

            entity.OrderDtls = list;

            _orderService.Add(entity);            
        }

        public void OrderUpdate(string orderId, Dictionary<string, object> dic)
        {
            var entity = _orderService.GetByOrderId(orderId);
            if (entity is null) return;

            _orderService.Update(entity, dic);
        }

        public JObject StringAnaly(string str, bool isSpecial = false)
        {
            if (string.IsNullOrEmpty(str)) return null;

            if (isSpecial)
            {
                str = str.Substring(str.IndexOf('?') + 1, str.Length - str.IndexOf('?') - 1);
                str = str.Replace("?", "&");
            }

            JObject obj = new JObject();

            string[] arr = str.Split('&');

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Split('=').Length != 2) continue;
                obj.Add(arr[i].Split('=')[0], arr[i].Split('=')[1]);
            }

            return obj;
        }
    }
}