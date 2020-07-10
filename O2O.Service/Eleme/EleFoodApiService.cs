using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.Common.Eleme;
using O2O.DTO.Eleme;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace O2O.Service.Eleme
{
    public class EleFoodApiService : EleBaseApiService
    {
        /// <summary>
        /// 查询店铺商品分类
        /// </summary>
        /// <param name="_shopId"></param>
        /// <returns></returns>
        public EleResult GetShopCategories(string token, long shopId)
        {
            var model = new { shopId = shopId };
            SignParams sign = GetSign(token, model, "eleme.product.category.getShopCategories");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 获取分类下所有菜品
        /// </summary>
        /// <param name="_categoryId"></param>
        /// <returns></returns>
        public EleResult GetItemsByCategoryId(string token, long categoryId)
        {
            var model = new { categoryId = categoryId };
            SignParams sign = GetSign(token, model, "eleme.product.item.getItemsByCategoryId");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 查看商品详情
        /// </summary>
        /// <param name="_itemId"></param>
        /// <returns></returns>
        public EleResult GetItem(string token, long itemId)
        {
            var model = new { itemId = itemId };
            SignParams sign = GetSign(token, model, "eleme.product.item.getItem");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 更新菜品
        /// eleme.product.item.updateItem
        /// 
        /// </summary>
        public EleResult UpdateItem(string token, dynamic product)
        {
            var model = new { itemId = product.itemId, categoryId = product.categoryId, properties = product.properties };
            SignParams sign = GetSign(token, model, "eleme.product.item.updateItem");
            string content = MakeNopEntity(sign, model);
            string res = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(res);
        }

        /// <summary>
        /// 批量更新商品库存
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public EleResult BatchUpdateStock(string token, Dictionary<string, int> stockMap)
        {
            var model = new { stockMap = stockMap };
            SignParams sign = GetSign(token, model, "eleme.product.item.batchUpdateStock");
            string content = MakeNopEntity(sign, model);
            string ret = HttpCommon.Post(EleConfig.API_URL, "application/json;charset=utf-8", null, content);
            return JsonConvert.DeserializeObject<EleResult>(ret);
        }
    }
}