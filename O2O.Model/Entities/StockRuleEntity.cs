using System.Collections.Generic;

namespace O2O.Model.Entities
{
    public class StockRuleEntity : BaseEntity
    { 
        /// <summary>
        /// �̻�
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Description { get; set; } 

        public virtual ICollection<StockRuleProdEntity> StockRuleProds { get; set; } = new List<StockRuleProdEntity>();

        public virtual ICollection<StockRuleShopEntity> StockRuleShops { get; set; } = new List<StockRuleShopEntity>();

    }
}
