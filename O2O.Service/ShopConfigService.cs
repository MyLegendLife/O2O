using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class  ShopConfigService : IShopConfigService
    {
        public async Task<ShopConfigDTO> GetAsync(string userId, string shopNo)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<ShopConfigEntity>(context);

                var entity = await service.Entities.FirstOrDefaultAsync(a => a.UserId == userId && a.ShopNo == shopNo);

                return entity is null ? new ShopConfigDTO() : ToolsCommon.EntityToEntity(entity, new ShopConfigDTO()) as ShopConfigDTO;
            }
        }

        public async Task<List<ShopConfigDTO>> GetListAsync(string userId)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<ShopConfigEntity>(context);

                return (await service.Entities
                    .Where(x => x.UserId == userId)
                    .ToListAsync())
                    .Select(x => ToolsCommon.EntityToEntity(x, new ShopConfigDTO()) as ShopConfigDTO)
                    .ToList();
            }
        }

        public async Task<List<ShopConfigDTO>> GetAutoSyncListAsync(string userId,int takeType)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<ShopConfigEntity>(context);

                if (takeType == 0)
                {
                    return (await service.Entities
                            .Where(x => x.UserId == userId && x.MtAutoSync == 1)
                            .ToListAsync())
                        .Select(x => ToolsCommon.EntityToEntity(x, new ShopConfigDTO()) as ShopConfigDTO)
                        .ToList();
                }
                else
                {
                    return (await service.Entities
                            .Where(x => x.UserId == userId && x.EleAutoSync == 1)
                            .ToListAsync())
                        .Select(x => ToolsCommon.EntityToEntity(x, new ShopConfigDTO()) as ShopConfigDTO)
                        .ToList();
                }
            }
        }

        public async Task UpdateAsync(List<ShopConfigDTO> inputs)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<ShopConfigEntity>(context);

                var userId = inputs[0].UserId;
                service.Delete(x => x.UserId == userId);

                service.AddRange(inputs.Select(x => ToolsCommon.EntityToEntity(x, new ShopConfigEntity()) as ShopConfigEntity));
            }
        }

        public async Task SetAsync(string userId, string shopNo, int mtState, int eleState)
        {
            using (var context = new O2OContext())
            {
                var service = new BaseService<ShopConfigEntity>(context);

                var entity = await service.Entities.FirstOrDefaultAsync(a => a.UserId == userId && a.ShopNo == shopNo);

                if (entity != null)
                {
                    entity.MtAutoConfirm = mtState;
                    entity.EleAutoConfirm = eleState;

                    service.Update(entity);
                }
                else
                {
                    var shopConfig = new ShopConfigEntity()
                    {
                        UserId = userId,
                        ShopNo = shopNo,
                        MtAutoConfirm = mtState,
                        EleAutoConfirm = eleState
                    };
                    service.Add(shopConfig);
                }
            }
        }
    }
}
