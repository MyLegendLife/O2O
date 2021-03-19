using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class StockRuleService : IStockRuleService
    {
        public async Task CreateAsync(StockRuleDTO stockRule, List<StockRuleProdDTO> stockRuleProd)
        {
            var entity = ToolsCommon.EntityToEntity(stockRule, new StockRuleEntity()) as StockRuleEntity;

            var list = stockRuleProd.Select(x =>
                ToolsCommon.EntityToEntity(x, new StockRuleProdEntity()) as StockRuleProdEntity).ToList();

            if (entity != null)
            {
                entity.StockRuleProds = list;

                using (var context = new O2OContext())
                {
                    var service = new BaseService<StockRuleEntity>(context);

                    service.Add(entity);
                }
            }
        }

        public async Task<List<StockRuleDTO>> GetListAsync(string userId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleEntity>(context);

                var entities = await service
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                var dtos = entities
                    .Select(x => ToolsCommon.EntityToEntity(x, new StockRuleDTO()) as StockRuleDTO)
                    .ToList();

                return dtos;
            }
        }

        public async Task<List<StockRuleProdDTO>> GetListProdAsync(Guid stockRuleId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleProdEntity>(context);

                var entities = await service.Where(x => x.StockRuleId == stockRuleId).ToListAsync();

                return entities
                    .Select(x => ToolsCommon.EntityToEntity(x, new StockRuleProdDTO()) as StockRuleProdDTO)
                    .ToList();
            }
        }

        public async Task<StockRuleProdDTO> GetProdAsync(Guid stockRuleId,string prodNo)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleProdEntity>(context);

                var entity = await service.Entities.FirstOrDefaultAsync(x => x.StockRuleId == stockRuleId && x.ProdNo == prodNo);

                return ToolsCommon.EntityToEntity(entity,new StockRuleProdDTO()) as StockRuleProdDTO;
            }
        }

        public async Task<List<StockRuleShopDTO>> GetListShopAsync(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleShopEntity>(context);

                var entities = await service.Where(x => x.StockRuleId == id).ToListAsync();

                return entities
                    .Select(x => ToolsCommon.EntityToEntity(x, new StockRuleShopDTO()) as StockRuleShopDTO)
                    .ToList();
            }
        }

        public async Task<List<StockRuleShopDTO>> GetListShopAsync(string userId)
        {
            var rules = new List<StockRuleEntity>();
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleEntity>(context);

                rules = await service.Where(x => x.UserId == userId).Include(x => x.StockRuleShops).ToListAsync();
            }

            return (
                from rule in rules
                from stockRuleShop in rule.StockRuleShops
                select new StockRuleShopDTO()
                {
                    ShopNo = stockRuleShop.ShopNo,
                    StockRuleId = rule.Id,
                    RuleName = rule.RuleName
                })
                .ToList();
        }

        public async Task<List<StockRuleShopProdDTO>> GetListShopProdAsync(string userId)
        {
            var rules = new List<StockRuleEntity>();
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleEntity>(context);

                rules = await service
                    .Where(x => x.UserId == userId)
                    .Include(x => x.StockRuleShops)
                    .Include(x => x.StockRuleProds)
                    .ToListAsync();
            }

            var shopProds = new List<StockRuleShopProdDTO>();
            foreach (var rule in rules)
            {
                foreach (var stockRuleShop in rule.StockRuleShops)
                {
                    shopProds.Add(new StockRuleShopProdDTO()
                    {
                        ShopNo = stockRuleShop.ShopNo,
                        StockRuleProds = rule.StockRuleProds.Select(x => ToolsCommon.EntityToEntity(x, new StockRuleProdDTO()) as StockRuleProdDTO).ToList()
                    });
                }
            }

            return shopProds;
        }

        public async Task<StockRuleShopDTO> GetShopAsync(string userId, string shopNo)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleEntity>(context);

                var entity = await (from a in service.Entities
                    from b in a.StockRuleShops
                    where a.UserId == userId && b.ShopNo == shopNo
                    select new StockRuleShopDTO()
                    {
                        ShopNo = b.ShopNo,
                        ShopName = "",
                        StockRuleId = b.StockRuleId,
                        RuleName = a.RuleName
                    })
                    .FirstOrDefaultAsync();


                return ToolsCommon.EntityToEntity(entity, new StockRuleShopDTO()) as StockRuleShopDTO;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleEntity>(context);

                service.Delete(id);
            }
        }

        public async Task CreateProdAsync(StockRuleProdDTO input)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleProdEntity>(context);

                var entity = ToolsCommon.EntityToEntity(input, new StockRuleProdEntity()) as StockRuleProdEntity;

                service.Add(entity);
            }
        }

        public async Task DeleteProdAsync(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleProdEntity>(context);

                service.Delete(id);
            }
        }

        public async Task CreateShopAsync(StockRuleShopDTO input)
        {

            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleShopEntity>(context);

                var entity = ToolsCommon.EntityToEntity(input, new StockRuleShopEntity()) as StockRuleShopEntity;

                service.Add(entity);
            }
        }

        public async Task UpdateShopAsync(Guid id, Guid newStockRuleId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleShopEntity>(context);

                var entity = service.GetById(id);

                entity.StockRuleId = newStockRuleId;

                service.Update(entity);
            }
        }

        public async Task DeleteShopAsync(Guid id)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<StockRuleShopEntity>(context);

                service.Delete(id);
            }
        }
    }
}
