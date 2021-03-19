using System;

namespace O2O.Model.Entities
{
    public class StockRuleShopEntity : BaseEntity
    { 
        /// <summary>
        /// �ŵ�
        /// </summary>
        public string ShopNo { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public Guid StockRuleId { get; set; }

        public virtual StockRuleEntity StockRule { get; set; }
    }
}
