using O2O.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace O2O.IService
{
    public interface IStockRuleService : IServiceSupport
    {
        Task CreateAsync(StockRuleDTO stockRule, List<StockRuleProdDTO> stockRuleProd);

        Task<List<StockRuleDTO>> GetListAsync(string userId);

        Task DeleteAsync(Guid id);



        Task<List<StockRuleProdDTO>> GetListProdAsync(Guid stockRuleId);

        Task<StockRuleProdDTO> GetProdAsync(Guid stockRuleId, string prodNo);

        Task CreateProdAsync(StockRuleProdDTO input);

        Task DeleteProdAsync(Guid id);



        Task CreateShopAsync(StockRuleShopDTO input);

        Task<List<StockRuleShopDTO>> GetListShopAsync(Guid id);

        Task<StockRuleShopDTO> GetShopAsync(string userId, string shopNo);

        Task<List<StockRuleShopDTO>> GetListShopAsync(string userId);

        Task UpdateShopAsync(Guid id, Guid newStockRuleId);

        Task DeleteShopAsync(Guid id);

        Task<List<StockRuleShopProdDTO>> GetListShopProdAsync(string userId);
    }
}
