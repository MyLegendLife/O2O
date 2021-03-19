using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO.Api
{
    public class FoodDataSellStatus
    {
        public string app_food_code { get; set; }

        public virtual ICollection<SkuSellStatus> skus { get; set; } = new List<SkuSellStatus>();
    }
}
