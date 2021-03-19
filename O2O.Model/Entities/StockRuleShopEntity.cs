using System;

namespace O2O.Model.Entities
{
    public class StockRuleShopEntity : BaseEntity
    { 
        /// <summary>
        /// √≈µÍ
        /// </summary>
        public string ShopNo { get; set; }

        /// <summary>
        /// πÊ‘Ú
        /// </summary>
        public Guid StockRuleId { get; set; }

        public virtual StockRuleEntity StockRule { get; set; }
    }
}
