using O2O.DTO;
using O2O.DTO.Meituan;
using O2O.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace O2O.IService
{
    public interface IPreProdService : IServiceSupport
    {
        List<PreProdDTO> Get(string userId);

        void Add(List<PreProdDTO> dtoList);

        void Delete(string userId);

        bool hasPreProd(string userId, string[] prodNos);
    }
}
