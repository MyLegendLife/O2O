using O2O.DTO.Eleme;
using System;
using System.Collections.Generic;

namespace O2O.IService
{
    public interface IEleShopService : IServiceSupport
    {
        void Add(Ele_ShopDTO dto);

        void Update(Ele_ShopDTO dto);

        bool Delete(long id);

        Ele_ShopDTO Get(string userId, string shopNo);

        Ele_ShopDTO GetByShopId(long shopId);

        List<Ele_ShopDTO> GetByAccountId(Guid accountId);

        List<Ele_ShopDTO> GetByUserId(string userId);
    }
}
