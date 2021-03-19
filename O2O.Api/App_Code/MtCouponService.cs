using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Api.Models.Meituan;
using O2O.Common;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Service;
using System;
using System.Linq;

namespace O2O.Api.App_Code
{
    public class MtCouponService
    {
        public string _tuangouAppKey;
        public string _tuangouAppSecret;
        public string _shopNo;

        public MtCouponService(string userNo, string shopNo)
        {
            IMtAccountService service = new MtAccountService();
            Mt_AccountDTO account = service.GetAccount(userNo);

            _tuangouAppKey = account.TuangouAppKey;
            _tuangouAppSecret = account.TuangouAppSecret;
            _shopNo = shopNo;
        }

        public Result Prepare(string codeNo)
        {
            var obj = new
            {
                couponCode = codeNo,
                vendorShopId = _shopNo
            };

            string url = GetUrl("https://open-api.dianping.com/tuangou/coupon/prepare", obj);

            var res = HttpCommon.Post(url, obj);

            var model = JsonConvert.DeserializeObject<BasicResponseModel>(res);

            if (model.code != 200)
            {
                return Tools.ResultErr(model.msg);
            }
            else
            {
                JObject content = JObject.FromObject(model.content);
                JArray jArray = JArray.Parse(content["dealMenu"][0].ToString());
                string prodList = "";
                for (int i = 0; i < jArray.Count; i++)
                {
                    prodList += (jArray[i]["content"] ?? "") + "  ";
                    prodList += (jArray[i]["specification"] ?? "") + "  ";
                    prodList += (jArray[i]["price"] ?? "") + "  ";
                    prodList += (jArray[i]["total"] ?? "") + ",";
                }

                var data = new
                {
                    CodeNo = codeNo,
                    Name = content["dealTitle"],
                    Type = content["dealType"],
                    Price = content["couponBuyPrice"],
                    EndTime = content["couponEndTime"],
                    Money = content["dealValue"],
                    ProdCount = jArray.Count,
                    ProdList = prodList
                };

                return Tools.ResultOk(data);
            }
        }

        public Result Query(string codeNo)
        {
            var obj = new
            {
                couponCode = codeNo,
                vendorShopId = _shopNo
            };

            string url = GetUrl("https://open-api.dianping.com/tuangou/coupon/status/query", obj);

            var res = HttpCommon.Post(url, obj);

            var model = JsonConvert.DeserializeObject<BasicResponseModel>(res);

            if (model.code != 200)
            {
                return Tools.ResultErr(model.msg);
            }
            else
            {
                var couponStatus = model.content as CouponStatusQueryResponse;

                var data = new
                {
                    CodeNo = codeNo,
                    Type = couponStatus.dealType,
                    Name = couponStatus.dealTitle,
                    Price = couponStatus.dealPrice,
                    Money = couponStatus.dealValue,
                    ProdCount = couponStatus.dealSkuMappingDetail.count,
                    ProdList = string.Join(",", couponStatus.dealSkuMappingDetail.vendorSkus)
                };

                return Tools.ResultOk(data);
            }
        }

        public Result Consume(JObject data)
        {
            string msg = "";

            string[] codes = data["Codes"].ToString().Trim(',').Split(',');

            for (int i = 0; i < codes.Length; i++)
            {
                try
                {
                    var model = new
                    {
                        vendorShopId = _shopNo,
                        couponCode = codes[i].ToString(),
                        count = 1,
                        eId = data["OperNo"],
                        eName = data["OperName"],
                        vendorOrderId = data["OrderID"]
                    };

                    string url = GetUrl("https://open-api.dianping.com/tuangou/coupon/consume", model);

                    var res = HttpCommon.Post(url, model);

                    JObject json = JObject.Parse(res);
                    if (json["code"].ToString() != "200")
                    {
                        msg += json["msg"];
                        break;
                    }
                }
                catch (Exception e)
                {
                    msg = e.Message;
                    break;
                }
            }

            if (msg != "")
            {
                return Tools.ResultErr(msg);
            }
            else
            {
                return Tools.ResultOk();
            }
        }
        
        public Result Cancel(JObject data)
        {
            string msgErr = "";
            string msgSuc = "";

            string[] codes = data["Codes"].ToString().Trim(',').Split(',');

            for (int i = 0; i < codes.Length; i++)
            {
                try
                {
                    var model = new
                    {
                        vendorShopId = _shopNo,
                        couponCode = codes[i].ToString(),
                        eId = data["OperNo"],
                        eName = data["OperName"],
                    };

                    string url = GetUrl("https://open-api.dianping.com/tuangou/coupon/cancel", model);

                    var res = HttpCommon.Post(url, model);

                    JObject json = JObject.Parse(res);
                    if (json["code"].ToString() == "200")
                    {
                        msgSuc += "成功" + "@";
                    }
                    else
                    {
                        msgSuc += "成功" + json["msg"] + "@";
                    }
                }
                catch (Exception e)
                {
                    msgErr = e.Message;
                    break;
                }
            }

            if (msgErr != "")
            {
                return Tools.ResultErr(msgErr);
            }
            else
            {
                return new Result() { State = "OK",Msg=msgSuc };
            }
        }

        public string GetUrl(string url, object model)
        {
            var modelSys = new
            {
                appKey = _tuangouAppKey,
                ts = ToolsCommon.GetTimestampInt32(),
                version = "2.0",
            };

            var sort = modelSys.GetType().GetProperties().OrderBy(a => a.Name);

            string str = "";
            string pas = "";
            foreach (var item in sort)
            {
                str += item.Name + item.GetValue(modelSys, null);
                pas += item.Name + "=" + item.GetValue(modelSys, null) + "&";
            }

            str = _tuangouAppSecret + str + JsonConvert.SerializeObject(model);

            string sign = ToolsCommon.SHA1Encrypt(str);

            return url + "?" + pas + "sign=" + sign;
        }
    }
}