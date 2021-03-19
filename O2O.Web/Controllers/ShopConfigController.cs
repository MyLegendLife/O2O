using Newtonsoft.Json;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Service;
using O2O.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class ShopConfigController : Controller
    {
        private readonly IShopConfigService _shopConfigService;

        public ShopConfigController(IShopConfigService shopConfigService)
        {
            _shopConfigService = shopConfigService;
        }

        public async Task<ActionResult> List()
        {
            //获取账户
            var bakShopList = Bak365Service.GetShopList();

            var list = await _shopConfigService.GetListAsync(Global.USER_ID);

            var data = from a in bakShopList
                join b in list on a.Key equals b.ShopNo into temp
                from x in temp.DefaultIfEmpty()
                select new ShopConfigDTO()
                {
                    ShopNo = a.Key,
                    ShopName = a.Value,
                    MtAutoConfirm = x?.MtAutoConfirm ?? 0,
                    EleAutoConfirm = x?.EleAutoConfirm ?? 0,
                    AutoSale = x?.AutoSale ?? 0,
                    MtAutoSync = x?.MtAutoSync ?? 0,
                    EleAutoSync = x?.EleAutoSync ?? 0
                };

            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Update(List<ShopConfigDTO> inputs)
        {
            foreach (var input in inputs)
            {
                input.UserId = Global.USER_ID;
            }

            await _shopConfigService.UpdateAsync(inputs);

            return Json(new AjaxResult() { state = "ok" });
        }
    }
}