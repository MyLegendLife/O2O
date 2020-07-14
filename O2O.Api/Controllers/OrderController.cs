using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.Models.Eleme;
using O2O.Api.Models.Meituan;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Service.Eleme;
using O2O.Service.Meituan;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Helpers;
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
            }
            catch (Exception e)
            {
                return Json(Tools.ResultErr(e.Message));
            }

            return Json(Tools.ResultOk(list));
        }

        [Route("SetBuy")]
        [HttpPost]
        public IHttpActionResult SetBuy(string userId, string shopNo, int takeType, string orderId)
        {
            try
            {
                var user = _userService.Get(userId);
                if (!string.IsNullOrWhiteSpace(user.SetBuyPara))
                {
                    if (takeType == 0)
                    {
                        MtOrderApiService service = new MtOrderApiService(userId);
                        var res = service.GetLogisticsStatus(orderId);
                        JObject obj = res.Data as JObject;
                        string logisticsStatus = obj["logistics_status"].ToString();
                        if (!user.SetBuyPara.Split(';')[0].Contains(logisticsStatus))
                        {
                            return Json(Tools.ResultErr("此订单暂时无法生成销售单"));
                        }
                    }
                    else if (takeType == 1)
                    {
                        EleOrderApiService service = new EleOrderApiService();
                        var shop = _shopEleService.Get(userId, shopNo);
                        string[] orderIds = { orderId };
                        var res = service.BatchGetDeliveryStates(shop.AccessToken, orderIds);
                        JObject obj = JObject.Parse(res.result.ToString());
                        string logisticsStatus = obj[orderId]["state"].ToString();
                        if (!user.SetBuyPara.Split(';')[1].Contains(logisticsStatus))
                        {
                            return Json(Tools.ResultErr("此订单暂时无法生成销售单"));
                        }
                    }
                }

                _orderService.SetBuy(orderId);
            }
            catch (Exception e)
            {
                return Json(Tools.ResultErr(e.Message));
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
                    MtOrderApiService service = new MtOrderApiService(userId, shopNo);
                    var res = service.Confirm(orderId);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 1);
                    }
                    return Json(res);
                }
                else
                {
                    EleOrderApiService service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.ConfirmOrderLite(shop.AccessToken, orderId);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【错误】类型:Confirm  信息{0}", e.Message);
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
                    MtOrderApiService service = new MtOrderApiService(userId, shopNo);
                    var res = service.Cancel(orderId, data);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 2);
                    }
                    return Json(res);
                }
                else
                {
                    EleOrderApiService service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.CancelOrderLite(shop.AccessToken, orderId, data);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【错误】类型:Cancel  信息{0}", e.Message);
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
                    MtOrderApiService service = new MtOrderApiService(userId, shopNo);
                    var res = service.RefundAgree(orderId, data);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 3);
                    }
                    return Json(res);
                }
                else
                {
                    EleOrderApiService service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.AgreeRefundLite(shop.AccessToken, orderId);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【错误】类型:RefundAgree  信息{0}", e.Message);
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
                    MtOrderApiService service = new MtOrderApiService(userId, shopNo);
                    var res = service.RefundReject(orderId, data);
                    if (res.State == "OK")
                    {
                        _orderService.UpdateState(orderId, 4);
                    }
                    return Json(res);
                }
                else
                {
                    EleOrderApiService service = new EleOrderApiService();
                    var shop = _shopEleService.Get(userId, shopNo);
                    var res = service.DisagreeRefundLite(shop.AccessToken, orderId, data);

                    return Json(Tools.ToResult(res));
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat("【错误】类型:RefundReject  信息{0}", e.Message);
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
                    MtOrderApiService service = new MtOrderApiService(userId, shopNo);
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
                _log.DebugFormat("【错误】类型:BatchPullPhoneNumber  信息{0}", e.Message);
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
                    MtOrderApiService service = new MtOrderApiService(userId, shopNo);

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
                _log.DebugFormat("【错误】类型:GetRiderInfoPhoneNumber  信息{0}", e.Message);
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
                return Json(Tools.ResultOk(ToolsCommon.GetEnumListValue(typeof(ElemeEnum.Cancel))));
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
    }
}