using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO.Api
{
    public class FoodData
    {
        public string app_food_code { get; set; }

        public virtual ICollection<Sku> skus { get; set; } = new List<Sku>();
    }
}
