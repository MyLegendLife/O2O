using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.App_Code;
using O2O.Api.Models.Eleme;
using O2O.Api.Models.Meituan;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Service;
using O2O.Service.Eleme;
using O2O.Service.Meituan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace O2O.Api.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        private static ILog _log = LogManager.GetLogger("Order");
        public IEleShopService _shopEleService { get; set; }
        public IOrderService _orderService { get; set; }
        public IUserService _userService { get; set; }

        [Route("GetOrder")]
        [HttpPost]
        public IHttpActionResult GetOrder(string userId, string shopNo, [FromBody] JObject query)
        {
            CreateMissOrderAll(userId, DateTime.Parse(query["OptTimeSta"].ToString()), shopNo, 0);
            CreateMissOrderAll(userId, DateTime.Parse(query["OptTimeSta"].ToString()), shopNo, 1);

            var list = new List<OrderDTO>();

            try
            {
                list = _orderService.GetOrders(userId, shopNo, query);
            }
            catch (Exception e)
            {
                return Json(Tools.ResultErr(e.Message));
            }

            return Json(Tools.ResultOk(list));
        }

        [Route("GetOrderDtl")]
        [HttpPost]
        public IHttpActionResult GetOrderDtl(string orderId)
        {
            var list = new List<OrderDtlDTO>();
            try
            {
                list = _orderService.GetOrderDtls(orderId);
                if (list.Count == 0)
                {
                    return Json(Tools.ResultErr("无明细数据"));
                }
            }
            catch (Exception e)
            {
                return Json(Tools.ResultErr(e.Message));
            }

            return Json(Tools.ResultOk(list));
        }

        [Route("GetOrderInfo")]
        [HttpPost]
        public IHttpActionResult GetOrderInfo(string orderId)
        {
            try
            {
                var entity = _orderService.GetByOrderId(orderId);
                if (entity is null)
                {
                    return Json(Tools.ResultErr("无此订单"));
                }
                else
                {
                    return Json(Tools.ResultOk(entity));
                }
            }
            catch (Exception e)
            {
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("CreateOrder")]
        [HttpPost]
        public IHttpActionResult CreateOrder(string userId,string shopNo,int takeType,string orderId)
        {
            try
            {
                if (_orderService.IsExist(orderId))
                    return Json(Tools.ResultErr("订单已经存在"));
                switch (takeType)
                {
                    case 0:
                        var orderDetail = new MtOrderApiService(userId, shopNo).GetOrderDetail(orderId);
                        if (orderDetail.State == "ERR")
                            return Json(Tools.ResultErr(orderDetail.Msg));
                        new MtCallBackService().CreateMissOrder(userId, orderDetail.Data.ToString());
                        _log.DebugFormat("【补单】用户:{0}    平台:美团     门店:{1}     单号:{2}", userId, shopNo, orderId);
                        break;
                    case 1:
                        var eleOrderApiService = new EleOrderApiService();
                        var eleShopDto = _shopEleService.Get(userId, shopNo);
                        var order = eleOrderApiService.GetOrder(eleShopDto.AccessToken, orderId);
                        if (order.error != null)
                            return Json(Tools.ResultErr(order.error.message));
                        new EleCallBackService().OrderNew(new MessageModel()
                        {
                            message = order.result.ToString(),
                            shopId = eleShopDto.ShopId
                        });
                        _log.DebugFormat("【补单】用户:{0} 平台:饿了么 门店:{1} 单号:{2}", userId, shopNo, orderId);
                        break;
                }
            }
            catch (Exception ex)
            {
                _log.DebugFormat("【错误】类型:CreateOrder  信息{0}", ex.Message);
                return Json(Tools.ResultErr("操作失败"));
            }
            return Json(Tools.ResultOk());
        }

        [Route("CreatePreOrder")]
        [HttpPost]
        public IHttpActionResult CreatePreOrder(string orderId)
        {
            try
            {
                //预定漏单补单,解决“未找到预订单”的问题
                var entity = _orderService.GetByOrderId(orderId);
                if (entity.OrderType == 1)
                {
                    Bak365Service.CreateBakOrder(entity, "Pre");
                }
            }
            catch (Exception ex)
            {
                return Json(Tools.ResultErr(ex.Message));
            }
            return Json(Tools.ResultOk());
        }

        [Route("SetBuy")]
        [HttpPost]
        public IHttpActionResult SetBuy(string userId, string shopNo, int takeType, string orderId,int buyState)
        {
            #region
            //try
            //{
            //    var user = _userService.Get(userId);
            //    if (!string.IsNullOrWhiteSpace(user.SetBuyPara))
            //    {
            //        if (takeType == 0)
            //        {
            //            MtOrderApiService service = new MtOrderApiService(userId);
            //            var res = service.GetLogisticsStatus(orderId);
            //            JObject obj = res.Data as JObject;
            //            string logisticsStatus = obj["logistics_status"].ToString();
            //            if (!user.SetBuyPara.Split(';')[0].Contains(logisticsStatus))
            //            {
            //                return Json(Tools.ResultErr("此订单暂时无法生成销售单"));
            //            }
            //        }
            //        else if (takeType == 1)
            //        {
            //            EleOrderApiService service = new EleOrderApiService();
            //            var shop = _shopEleService.Get(userId, shopNo);
            //            string[] orderIds = { orderId };
            //            var res = service.BatchGetDeliveryStates(shop.AccessToken, orderIds);
            //            JObject obj = JObject.Parse(res.result.ToString());
            //            string logisticsStatus = obj[orderId]["state"].ToString();
            //            if (!user.SetBuyPara.Split(';')[1].Contains(logisticsStatus))
            //            {
            //                return Json(Tools.ResultErr("此订单暂时无法生成销售单"));
            //            }
            //        }
            //    }

            //    _orderService.SetBuy(orderId);
            //}
            //catch (Exception e)
            //{
            //    return Json(Tools.ResultErr(e.Message));
            //}

            //return Json(Tools.ResultOk());

            #endregion
            try
            {
                //UserDTO userDTO = _userService.Get(userId);
                //if (string.IsNullOrWhiteSpace(userDTO.SetBuyPara) || takeType != 0)
                //{
                //}

                _orderService.SetBuy(orderId, buyState);
            }
            catch (Exception ex)
            {
                return Json(Tools.ResultErr(ex.Message));
            }
            return Json(Tools.ResultOk());
        }

        [Route("Confirm")]
        [HttpPost]
        public IHttpActionResult Confirm(string userId, string shopNo, int takeType, string orderId)
        {
            try
            {
                if (takeType == 0)
                {
                    var service = new MtOrderApiService(userId, shopNo);
                    var res = service.Confirm(orderId);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 1);
                    }
                    return Json(res);
                }
                else
                {
                    var service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.ConfirmOrderLite(shop.AccessToken, orderId);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:Confirm 单号：{0} 错误{1}", orderId, e.Message);
                _log.DebugFormat("【系统错误】类型:Confirm 单号：{0} 错误{1}", orderId, e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("Cancel")]
        [HttpPost]
        public IHttpActionResult Cancel(string userId, string shopNo, int takeType, string orderId, [FromBody] JObject data)
        {
            try
            {
                if (takeType == 0)
                {
                    var service = new MtOrderApiService(userId, shopNo);
                    var res = service.Cancel(orderId, data);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 2);
                    }
                    return Json(res);
                }
                else
                {
                    var service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.CancelOrderLite(shop.AccessToken, orderId, data);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:Cancel  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:Cancel  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("RefundAgree")]
        [HttpPost]
        public IHttpActionResult RefundAgree(string userId, string shopNo, int takeType, string orderId, [FromBody] JObject data)
        {
            try
            {
                if (takeType == 0)
                {
                    var service = new MtOrderApiService(userId, shopNo);
                    var res = service.RefundAgree(orderId, data);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 3);
                    }
                    return Json(res);
                }
                else
                {
                    var service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.AgreeRefundLite(shop.AccessToken, orderId);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:RefundAgree  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:RefundAgree  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("RefundReject")]
        [HttpPost]
        public IHttpActionResult RefundReject(string userId, string shopNo, int takeType, string orderId, [FromBody] JObject data)
        {
            try
            {
                if (takeType == 0)
                {
                    var service = new MtOrderApiService(userId, shopNo);
                    var res = service.RefundReject(orderId, data);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 4);
                    }
                    return Json(res);
                }
                else
                {
                    var service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.DisagreeRefundLite(shop.AccessToken, orderId, data);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:RefundReject  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:RefundReject  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("BatchPullPhoneNumber")]
        [HttpPost]
        public IHttpActionResult BatchPullPhoneNumber(string userId, string shopNo, int takeType)
        {
            try
            {
                if (takeType == 0)
                {
                    var service = new MtOrderApiService(userId, shopNo);
                    var res = service.BatchPullPhoneNumber();

                    return Json(res);
                }
                else
                {
                    return Json(new Result());
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:BatchPullPhoneNumber  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:BatchPullPhoneNumber  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("GetRiderInfoPhoneNumber")]
        [HttpPost]
        public IHttpActionResult GetRiderInfoPhoneNumber(string userId, string shopNo, int takeType)
        {
            try
            {
                if (takeType == 0)
                {
                    var service = new MtOrderApiService(userId, shopNo);

                    var res = service.GetRiderInfoPhoneNumber();

                    return Json(res);
                }
                else
                {
                    return Json(new Result());
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【系统错误】类型:GetRiderInfoPhoneNumber  信息{0}", e.Message);
                _log.DebugFormat("【系统错误】类型:GetRiderInfoPhoneNumber  信息{0}", e.GetOriginalException().Message);
                return Json(Tools.ResultErr(e.Message));
            }
        }

        [Route("GetCancelState")]
        [HttpPost]
        public IHttpActionResult GetCancelState(int takeType)
        {
            if (takeType == 0)
            {
                return Json(Tools.ResultOk(ToolsCommon.GetEnumListValue(typeof(MeituanEnum.Cancel))));
            }
            else if (takeType == 1)
            {
                return Json(Tools.ResultOk(ToolsCommon.GetEnumListValue(typeof(ElemeEnum.Refund))));
            }

            return Json(Tools.ResultOk());
        }

        [Route("GetRefundState")]
        [HttpPost]
        public IHttpActionResult GetRefundState(int takeType)
        {
            if (takeType == 0)
            {
                return Json(Tools.ResultOk(ToolsCommon.GetEnumListKey(typeof(MeituanEnum.Refund))));
            }
            else if (takeType == 1)
            {
                return Json(Tools.ResultOk(ToolsCommon.GetEnumListKey(typeof(ElemeEnum.Refund))));
            }

            return Json(Tools.ResultOk());
        }

        #region 饿了么补单 2021-03-12

        //[Route("GetGet")]
        //[HttpPost]
        //public IHttpActionResult GetGet()
        //{
        //    var eleOrderApiService = new EleOrderApiService();
        //    var eleCallBackService = new EleCallBackService();

        //    var shops = _shopEleService.GetByUserId("0002");

        //    foreach (var shop in shops)
        //    {
        //        if (shop is null) continue;

        //        var orders = eleOrderApiService.GetAllOrders("18c0a5787a2639347c0e493947d515c6", shop.ShopId, 1, 50, "2021-03-12");
        //        if (orders.error != null)
        //            continue;

        //        var models = JsonConvert.DeserializeObject<List<Models.Eleme.OrderModel>>(JObject.Parse(orders.result.ToString())["list"].ToString());

        //        using (var enumerator = models.GetEnumerator())
        //        {
        //            while (enumerator.MoveNext())
        //            {
        //                var current = enumerator.Current;
        //                var message = new MessageModel()
        //                {
        //                    message = JsonConvert.SerializeObject(current),
        //                    shopId = shop.ShopId
        //                };
        //                eleCallBackService.OrderNew(message);
        //            }
        //        }
        //    }

        //    return Json(Tools.ResultOk());
        //}

        #endregion

        public void CreateMissOrderAll(string userId, DateTime dateTime, string shopNo, int takeType)
        {
            try
            {
                var missDaySeqArrary = _orderService.GetMissOrder(userId, dateTime, shopNo, takeType);
                switch (takeType)
                {
                    case 0:
                        var mtOrderApiService = new MtOrderApiService(userId, shopNo);
                        foreach (var daySeq in missDaySeqArrary)
                        {
                            var orderIdByDaySeq = mtOrderApiService.GetOrderIdByDaySeq(dateTime, daySeq);
                            if (!(orderIdByDaySeq.State == "ERR"))
                            {
                                var jtoken = JObject.Parse(orderIdByDaySeq.Data.ToString())["order_id"];
                                var orderDetail = mtOrderApiService.GetOrderDetail(jtoken.ToString());
                                if (!(orderDetail.State == "ERR"))
                                {
                                    var mtCallBackService = new MtCallBackService();
                                    _log.DebugFormat("【补单】商户{0} 平台：美团 单号：{1}", userId, jtoken);
                                    mtCallBackService.CreateMissOrder(userId, orderDetail.Data.ToString());
                                }
                            }
                        }
                        break;
                    case 1:
                        var eleOrderApiService = new EleOrderApiService();
                        var eleShopDto = _shopEleService.Get(userId, shopNo);
                        if (eleShopDto is null) return;

                        var allOrders = eleOrderApiService.GetAllOrders(eleShopDto.AccessToken, eleShopDto.ShopId, 1, 50, dateTime.ToString("yyyy-MM-dd"));
                        if (allOrders.error != null)
                            break;
                        var orderModels = JsonConvert.DeserializeObject<List<Models.Eleme.OrderModel>>(JObject.Parse(allOrders.result.ToString())["list"].ToString()).Where(a => missDaySeqArrary.Contains(a.daySn));
                        var eleCallBackService = new EleCallBackService();
                        using (var enumerator = orderModels.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                var current = enumerator.Current;
                                var message = new MessageModel()
                                {
                                    message = JsonConvert.SerializeObject(current),
                                    shopId = eleShopDto.ShopId
                                };
                                _log.DebugFormat("【补单】商户{0} 平台：饿了么 单号：{1}", userId, current?.orderId);
                                eleCallBackService.OrderNew(message);
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                _log.DebugFormat("【系统错误】类型:CreateAllOrder 门店:{0}  信息{1}", shopNo,ex.Message);
            }
        }
    }
}