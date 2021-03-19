using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.Models.Meituan;
using O2O.Common;
using O2O.IService;
using O2O.Model.Entities;
using O2O.Service;
using O2O.Service.Meituan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace O2O.Api
{
    public class MtCallBackService
    {
        private IOrderService _orderService { get; set; }
        private IPreProdService _preProdService { get; set; }
        private IShopConfigService _shopConfigService { get; set; }
        private static ILog _log = LogManager.GetLogger("MtCallBackService");

        public MtCallBackService()
        {
            _orderService = new OrderService();
            _preProdService = new PreProdService();
            _shopConfigService = new ShopConfigService();
        }

        public void HandlePush(string type, string userId, string res)
        {
            if (string.IsNullOrEmpty(res)) return;

            var isSpecial = type == "OrderCanceled";

            var obj = StringAnaly(res, isSpecial);
            if (obj == null || obj.Count <= 1) return; //处理心跳

            var order = _orderService.GetByOrderId(obj["order_id"].ToString());

            var fields = new List<string>();
            switch (type)
            {
                case "OrderFinished":
                    if (order is null)
                    {
                        OrderPayed(userId, res);
                        return;
                    }
                    fields.Add("State");
                    order.State = 4;
                    break;
                case "OrderCanceled":
                    if (order is null) return;
                    order.CancelCode = obj["reason_code"].ToString();
                    order.CancelReason = obj["reason"].ToString();
                    order.State = 2;
                    fields.Add("CancelCode");
                    fields.Add("CancelReason");
                    fields.Add("State");
                    Bak365Service.SendBakNotice(obj[nameof(userId)].ToString(), obj["order_id"]?.ToString(), order.ShopNo, 1);
                    break;
                case "OrderDeliveringStatus":
                    if (order is null) return;
                    var logisticsStatus = (MeituanEnum.LogisticsStatus)Enum.Parse(typeof(MeituanEnum.LogisticsStatus), obj["logistics_status"].ToString());
                    if (logisticsStatus != MeituanEnum.LogisticsStatus.F)
                    {
                        order.State = logisticsStatus.GetOrder();
                        fields.Add("State");
                        if (!string.IsNullOrWhiteSpace(obj["dispatcher_mobile"].ToString()))
                        {
                            order.DispatcherName = obj["dispatcher_name"].ToString();
                            order.DispatcherMobile = obj["dispatcher_mobile"].ToString();
                            fields.Add("DispatcherName");
                            fields.Add("DispatcherMobile");
                        }
                    }
                    //骑手已经取餐&开启自动生成销售单
                    if (logisticsStatus == MeituanEnum.LogisticsStatus.D)
                    {
                        var shopConfig = _shopConfigService.GetAsync(order.UserId, order.ShopNo).Result;
                        if (shopConfig.AutoSale == 1)
                        {
                            if (order.OrderType == 0) //现购
                            {
                                Bak365Service.CreateBakOrder(order, "Sale");
                                order.BuyState = 1;
                                fields.Add("BuyState");
                            }
                            else if (order.OrderType == 1) //预定
                            {
                                Bak365Service.FinPreOrder(order.UserId, order.OrderId);
                                order.BuyState = 3;
                                fields.Add("BuyState");
                            }
                        }
                    }
                    break;
            }
            _orderService.UpdateEntityFields(order, fields);
        }

        public void OrderPayed(string userId, string res)
        {
            var obj = StringAnaly(res);
            if (obj == null || obj.Count <= 1) return;

            //obj["recipient_address"] = ToolsCommon.MidStrEx(res, "&recipient_address=", "&taxpayer_id");
            //obj["caution"] = ToolsCommon.MidStrEx(res, "&caution=", "&order_id");

            var model = JsonConvert.DeserializeObject<OrderModel>(obj.ToString());
            CreateOrder(userId, model);

            //是否自动接单
            var shopConfig = _shopConfigService.GetAsync(userId, model.app_poi_code).Result;
            if (shopConfig.MtAutoConfirm == 0)
            {
                //var controller = new OrderController();
                //controller.Confirm(userId, model.app_poi_code, 0, model.order_id);
                var service = new MtOrderApiService(userId, model.app_poi_code);
                var resConfirm = service.Confirm(model.order_id);
                if (resConfirm.State == "OK")
                {
                    _orderService.UpdateState(model.order_id, 1);
                }
            }
        }

        public async void OrderConfirmed(string userId, string res)
        {
            //避免跟Payed推送并发,造成状态异常
            await Task.Delay(10000);

            var obj = StringAnaly(res);
            if (obj == null || obj.Count <= 1) return;

            var order = _orderService.GetByOrderId(obj["order_id"].ToString());
            if (order != null)
            {
                if (order.State != 0)
                {
                    return;
                }

                List<string> fileds = new List<string>() { "State" };

                order.State = 1;
                _orderService.UpdateEntityFields(order, fileds);
            }
            else
            {
                //obj["recipient_address"] = ToolsCommon.MidStrEx(res, "&recipient_address=", "&taxpayer_id");
                //obj["caution"] = ToolsCommon.MidStrEx(res, "&caution=", "&order_id");
                var model = JsonConvert.DeserializeObject<OrderModel>(obj.ToString());
                CreateOrder(userId, model);
            }
        }

        private void CreateOrder(string userId, OrderModel model)
        {
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
            var detailModelList = JsonConvert.DeserializeObject<List<DetailModel>>(model.detail);

            var dtls = new List<OrderDtlEntity>();

            foreach (var detailModel in detailModelList)
            {
                //如果商品未映射，且价格为0，则忽略,排除广告
                if (string.IsNullOrWhiteSpace(detailModel.sku_id) && detailModel.price == 0)
                {
                    continue;
                }

                var prodCountList = detailModel.sku_id.Trim('X').Split('X').
                    GroupBy(a => a).
                    Select(b => new
                    {
                        ProdNo = b.Key,
                        Count = b.Count()
                    });

                foreach (var prodCount in prodCountList)
                {
                    //dtls.Add(new OrderDtlEntity()
                    //{
                    //    ProdNo = prodCount.ProdNo,
                    //    ProdName = detailModel.food_name + detailModel.food_property + detailModel.spec,
                    //    ProdUnit = detailModel.unit,
                    //    Price = detailModel.price / prodCount.Count,
                    //    ItemCnt = detailModel.quantity * prodCount.Count,
                    //    ItemSum = detailModel.price * detailModel.quantity
                    //});

                    //改为取365商品信息，如果未匹配上365商品，则取美团商品信息
                    var prod = Bak365Service.GetProd(userId, prodCount.ProdNo);

                    dtls.Add(new OrderDtlEntity()
                    {
                        ProdNo = prod.ProdNo ?? "",
                        ProdName = (prod.ProdName ?? detailModel.food_name) + " " + detailModel.food_property + detailModel.spec,
                        ProdUnit = prod.ProdUnit ?? detailModel.unit,
                        Price = double.Parse(prod.Price ?? detailModel.price.ToString()),
                        ItemCnt = detailModel.quantity * prodCount.Count,
                        ItemSum = double.Parse(prod.Price ?? detailModel.price.ToString()) * detailModel.quantity * prodCount.Count
                    });
                }
            }

            entity.OrderDtls = dtls;

            try
            {
                //判断商品中是否有预订商品
                if (_preProdService.hasPreProd(userId, dtls.Select(a => a.ProdNo).ToArray()))
                {
                    entity.OrderType = 1;
                    entity.BuyState = 2; //0未生成现购单 1已生成现购单 2已生成预定单 3已提货 4已预定作废

                    Bak365Service.CreateBakOrder(entity, "Pre");
                }
            }
            catch (Exception ex)
            {
                _log.DebugFormat("【系统错误】类型:CreateOrder 信息:{0} 错误:{1}", JsonConvert.SerializeObject(model), ex.Message);
            }
            finally
            {
                _orderService.Add(entity);
                //发送通知
                Bak365Service.SendBakNotice(entity.UserId, entity.OrderId, entity.ShopNo);
            }
        }

        public void CreateMissOrder(string userId, string res)
        {
            var model = JsonConvert.DeserializeObject<OrderModel>(res);
            CreateOrder(userId, model);
        }

        public void OrderRefund(string res)
        {
            var obj = StringAnaly(res, true);

            if (obj.Count <= 1) return;

            CacheHelper.Set(obj["order_id"]?.ToString() + "_" + obj["notify_type"]?.ToString(), "");
            var entity = _orderService.GetByOrderId(obj["order_id"].ToString());
            if (entity == null) return;

            entity.RefundCode = obj["notify_type"].ToString();
            entity.RefundReason = obj["reason"].ToString();
            var refund = (MeituanEnum.Refund)Enum.Parse(typeof(MeituanEnum.Refund), obj["notify_type"].ToString());
            entity.State = refund.GetOrder();

            if (refund == MeituanEnum.Refund.apply && CacheHelper.GetContainsKeyCount(obj["order_id"]?.ToString()) > 1) return;

            _orderService.Update(entity);
            Bak365Service.SendBakNotice(obj["userId"].ToString(), obj["order_id"]?.ToString(), entity.ShopNo, 1);
        }

        public void OrderRefundPart(string res)
        {
            var obj = StringAnaly(res, true);

            if (obj.Count <= 1) return;

            CacheHelper.Set(obj["order_id"] + "_" + obj["notify_type"], "");

            var entity = _orderService.GetByOrderId(obj["order_id"].ToString());
            if (entity == null) return;

            entity.RefundCode = obj["notify_type"].ToString();
            entity.RefundReason = obj["reason"].ToString();
            var refund = (MeituanEnum.Refund)Enum.Parse(typeof(MeituanEnum.Refund), obj["notify_type"].ToString());
            entity.State = refund.GetOrder();

            //缓存中存在订单的后续推送信息,则不处理
            if (refund == MeituanEnum.Refund.part && CacheHelper.GetContainsKeyCount(obj["order_id"]?.ToString()) > 1) return;

            if (refund == MeituanEnum.Refund.agree)
            {
                entity.State = 4;
                entity.RefundPartAmt = double.Parse(obj["money"].ToString());

                var refundDtls = JsonConvert.DeserializeObject<List<RefundPartDetailModel>>(obj["food"].ToString());

                foreach (var dtl in refundDtls)
                {
                    var orderDtlEntity = entity.OrderDtls.FirstOrDefault(a => a.ProdNo == dtl.sku_id);
                    if (orderDtlEntity != null)
                    {
                        orderDtlEntity.RefundPartCnt = dtl.count;
                    }
                }
            }

            _orderService.Update(entity);
            _orderService.UpdateDtl(entity.OrderDtls);
            Bak365Service.SendBakNotice(obj["userId"].ToString(), obj["order_id"]?.ToString(), entity.ShopNo, 1);
        }

        public JObject StringAnaly(string str, bool isSpecial = false)
        {
            if (string.IsNullOrEmpty(str)) return null;

            if (isSpecial)
            {
                str = str.Substring(str.IndexOf('?') + 1, str.Length - str.IndexOf('?') - 1);
                str = str.Replace("?", "&");
            }
            var jobject = new JObject();
            var strArray = str.Split('&');

            for (int index = 0; index < strArray.Length; ++index)
            {
                if (strArray[index].Split('=').Length == 2)
                    jobject.Add(strArray[index].Split('=')[0], strArray[index].Split('=')[1]);
            }
            return jobject;
        }
    }
}