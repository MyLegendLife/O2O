using log4net;
using Newtonsoft.Json;
using O2O.BackgroundJobs.Models;
using O2O.BackgroundJobs.Services;
using O2O.BackgroundJobs.Utils;
using O2O.Common;
using O2O.DTO;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Service.Eleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace O2O.BackgroundJobs.Jobs
{
    public class UpDownJob
    {
        public readonly IStockRuleService _stockRuleService;
        private readonly IUserService _userService;
        public readonly IMtAccountService _accountService;
        public readonly IEleShopService _eleShopService;
        public readonly IJobService _jobService;
        private static ILog _log = LogManager.GetLogger("UpDownJob");

        public UpDownJob(IStockRuleService stockRuleService, IUserService userService,
            IMtAccountService accountService, IEleShopService eleShopService, IJobService jobService)
        {
            _stockRuleService = stockRuleService;
            _userService = userService;
            _accountService = accountService;
            _eleShopService = eleShopService;
            _jobService = jobService;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var users = _userService.GetAll();

                foreach (var user in users)
                {
                    await UpDown(user);
                }
            }
            catch (Exception e)
            {
                _log.DebugFormat($"【系统错误】：{e.Message}");
            }
        }

        public async Task UpDown(UserDTO user)
        {
            var account = _accountService.GetAccount(user.Id);
            var bak365Util = new Bak365Util(user.ConnString);

            var shopProds = await _stockRuleService.GetListShopProdAsync(user.Id);

            foreach (var shopProd in shopProds)
            {
                var currentStocks =
                    bak365Util.GetCurrentStock(shopProd.ShopNo, shopProd.StockRuleProds.Select(x => x.ProdNo));

                var data = from a in shopProd.StockRuleProds
                           join b in currentStocks on a.ProdNo equals b.ProdNo
                           select new StockModel()
                           {
                               ProdNo = a.ProdNo,
                               MtMarkStock = a.MtStock,
                               EleMarkStock = a.EleStock,
                               CurrentStock = b.Stock
                           };

                await MtUpDown(account, shopProd.ShopNo, data);
                await EleUpDown(user.Id, shopProd.ShopNo, data);
            }
        }

        //售卖状态，1表下架，0表上架
        private async Task MtUpDown(Mt_AccountDTO account, string shopNo, IEnumerable<StockModel> models)
        {
            //上架
            var upData = models.Where(x => x.CurrentStock > x.MtMarkStock);
            if (!upData.Any()) return;
            await _jobService.MtUpdateState(account.WaimaiAppId, account.WaimaiAppSecret, shopNo, 0, upData);

            //下架
            var downData = models.Where(x => x.CurrentStock <= x.MtMarkStock);
            if (!downData.Any()) return;
            await _jobService.MtUpdateState(account.WaimaiAppId, account.WaimaiAppSecret, shopNo, 1, downData);

            //上架时更新库存
            {
                var foods = await _jobService.MtFoods(account.WaimaiAppId, account.WaimaiAppSecret, shopNo);

                //结合接口获取的商品，得到ProdCode
                var data = from a in upData
                    join b in foods on a.ProdNo equals b.ProdNo
                    select new StockModel()
                    {
                        ProdCode = b.ProdCode,
                        ProdNo = a.ProdNo,
                        CurrentStock = a.CurrentStock
                    };

                await _jobService.MtUpdateStock(account.WaimaiAppId, account.WaimaiAppSecret, shopNo, data);
            }
        }

        //售卖状态，1表下架，0表上架
        private async Task EleUpDown(string userId, string shopNo, IEnumerable<StockModel> models)
        {
            var service = new EleFoodApiService();

            var shop = _eleShopService.Get(userId, shopNo);
            if (shop == null) return;

            //通过接口查询门店商品，然后转化成 Id对应ProdNo
            var res = service.QueryItemByPage(shop.AccessToken, shop.ShopId);
            if (res.error != null) return;

            var oItems = JsonConvert.DeserializeObject<List<OItem>>(res.result.ToString());
            var idNos = (
                from oItem in oItems
                from spec in oItem.specs
                select new
                {
                    Id = oItem.id,
                    specId = spec.specId,
                    ProdNo = spec.extendCode
                })
                .ToList();


            //匹配对应的SpecId
            var data = from a in models
                       join b in idNos on a.ProdNo equals b.ProdNo
                       select new StockModel
                       {
                           ProdId = b.Id,
                           SpecId = b.specId,
                           ProdNo = b.ProdNo,
                           CurrentStock = a.CurrentStock,
                           EleMarkStock = a.EleMarkStock
                       };

            var size = 50;  //分批次提交

            //上架 提交商品Id
            {
                var upData = data.Where(x => x.CurrentStock > x.EleMarkStock).Select(x => x.ProdId);
                if (!upData.Any()) return;
                var loop = (upData.Count() + size - 1) / size;
                for (var i = 0; i < loop; i++)
                {
                    var response = service.BatchListItems(shop.AccessToken, upData.Skip(i * size).Take(size).ToList());
                }
            }

            //下架 提交商品Id
            {
                var downData = data.Where(x => x.CurrentStock <= x.EleMarkStock).Select(x => x.ProdId);
                if (!downData.Any()) return;
                var loop = (downData.Count() + size - 1) / size;
                for (var i = 0; i < loop; i++)
                {
                    var response = service.BatchDelistItems(shop.AccessToken, downData.Skip(i * size).Take(size).ToList());
                }
            }

            //上架时更新库存 提交商品规格Id
            {
                var upData = data.Where(x => x.CurrentStock > x.EleMarkStock);
                if (!upData.Any()) return;

                var loop = (upData.Count() + size - 1) / size;

                for (var i = 0; i < loop; i++)
                {
                    var dic = upData.Skip(i * size).Take(size).ToDictionary(x => x.SpecId, x => ToolsCommon.IsPositiveNumber(x.CurrentStock.ToString()) ? (int)x.CurrentStock : 0);

                    var response = service.BatchUpdateStock(shop.AccessToken, dic);
                }
            }
        }
    }
}
