using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using O2O.BackgroundJobs.Models;
using O2O.DTO.Api;

namespace O2O.BackgroundJobs.Services
{
    public interface IJobService
    {
        Task MtUpdateState(string waimaiAppId, string waimaiAppSecret, string shopNo, int state, IEnumerable<StockModel> models);

        Task MtUpdateStock(string waimaiAppId, string waimaiAppSecret, string shopNo, IEnumerable<StockModel> models);

        Task<IEnumerable<StockModel>> MtFoods(string waimaiAppId, string waimaiAppSecret, string shopNo);
        
        string GetUrl(string appSecret, string url, JObject model);
    }
}