using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Model;
using O2O.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace O2O.Service
{
    public class PreProdService : IPreProdService
    {
        public void Add(List<PreProdDTO> list)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<PreProdEntity> service = new BaseService<PreProdEntity>(context);

                var entityList = list.Select(a => ToolsCommon.EntityToEntity(a, new PreProdEntity()) as PreProdEntity).ToList();

                service.AddRange(entityList);
            }
        }

        public void Delete(string userId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<PreProdEntity> service = new BaseService<PreProdEntity>(context);

                service.Delete(a => a.UserId == userId);
            }
        }

        public List<PreProdDTO> Get(string userId)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<PreProdEntity> service = new BaseService<PreProdEntity>(context);

                var list = service
                    .Where(a => a.UserId == userId).ToList()
                    .Select(a => ToolsCommon.EntityToEntity(a, new PreProdDTO()) as PreProdDTO)
                    .ToList();

                return list;
            }
        }

        public bool hasPreProd(string userId,string[] prodNos)
        {
            using (O2OContext context = new O2OContext())
            {
                BaseService<PreProdEntity> service = new BaseService<PreProdEntity>(context);

                var count = service.Where(a => a.UserId == userId && prodNos.Contains(a.ProdNo)).Count();

                return count > 0 ? true : false;
            }
        }
    }
}
