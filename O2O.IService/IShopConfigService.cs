using O2O.DTO;
using O2O.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace O2O.IService
{
    public interface IShopConfigService : IServiceSupport
    {
        Task<ShopConfigDTO> GetAsync(string userId, string shopNo);

        Task<List<ShopConfigDTO>> GetListAsync(string userId);

        Task<List<ShopConfigDTO>> GetAutoSyncListAsync(string userId, int takeType);

        Task UpdateAsync(List<ShopConfigDTO> inputs);

        Task SetAsync(string userId, string shopNo, int mtState, int eleState);
    }
}
