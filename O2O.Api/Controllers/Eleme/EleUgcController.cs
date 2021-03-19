using Newtonsoft.Json.Linq;
using O2O.Api.Models.Eleme;
using O2O.Common;
using O2O.IService;
using O2O.Service.Eleme;
using System.Collections.Generic;
using System.Web.Http;

namespace O2O.Api.Controllers.Eleme
{
    [RoutePrefix("api/EleUgc")]
    public class EleUgcController : ApiController
    {
        public IEleShopService eleShopService { get; set; }

        [Route("CountRateResult")]
        [HttpPost]
        public IHttpActionResult CountRateResult(JObject data)
        {
            var userId = data["userId"]?.ToString();
            var shopNo = data["shopNo"]?.ToString();
            var startTime = data["startTime"]?.ToString();
            var endTime = data["endTime"]?.ToString();
            var starRating = data["starRating"]?.ToObject<List<int>>();
            var dataType = data["dataType"]?.ToString();

            var shop = eleShopService.Get(userId, shopNo);

            if (shop == null) return Json(Tools.ResultErr("商户编号或门店编号错误"));

            var rateQuery = new ORateQuery()
            {
                shopId = shop.ShopId,
                startTime = startTime,
                endTime = endTime,
                starRating = starRating,
                dataType = dataType
            };

            var service = new EleUgcApiService();

            var response = service.CountORateResult(shop.AccessToken, rateQuery);

            if (response.error != null)
            {
                return Json(Tools.ResultErr(response.error.message));
            }

            return Json(Tools.ResultOk(response.result));
        }

        [Route("GetOrderRatesByShopId")]
        [HttpPost]
        public IHttpActionResult GetOrderRatesByShopId(JObject data)
        {
            var userId = data["userId"]?.ToString();
            var shopNo = data["shopNo"]?.ToString();
            var startTime = data["startTime"]?.ToString();
            var endTime = data["endTime"]?.ToString();
            int.TryParse(data["offset"]?.ToString(),out var offset);
            int.TryParse(data["pageSize"]?.ToString(), out var pageSize);

            var shop = eleShopService.Get(userId, shopNo);

            if (shop == null) return Json(Tools.ResultErr("商户编号或门店编号错误"));

            var service = new EleUgcApiService();
            
            var response = service.GetOrderRatesByShopId(shop.AccessToken, shop.ShopId, startTime, endTime, offset, pageSize);
            if (response.error != null)
            {
                return Json(Tools.ResultErr(response.error.message));
            }

            return Json(Tools.ResultOk(response.result));
        }

        [Route("GetORateResult")]
        [HttpPost]
        public IHttpActionResult GetORateResult(JObject data)
        {
            var userId = data["userId"]?.ToString();
            var shopNo = data["shopNo"]?.ToString();
            var startTime = data["startTime"]?.ToString();
            var endTime = data["endTime"]?.ToString();
            int.TryParse(data["offset"]?.ToString(), out var offset);
            int.TryParse(data["pageSize"]?.ToString(), out var pageSize);

            var shop = eleShopService.Get(userId, shopNo);

            if (shop == null) return Json(Tools.ResultErr("商户编号或门店编号错误"));

            var service = new EleUgcApiService();

            var response = service.GetORateResult(shop.AccessToken, shop.ShopId, startTime, endTime, offset, pageSize);
            if (response.error != null)
            {
                return Json(Tools.ResultErr(response.error.message));
            }

            return Json(Tools.ResultOk(response.result));
        }
    }
}