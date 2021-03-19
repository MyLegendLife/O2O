using System.Collections.Generic;

namespace O2O.Model.Entities
{
    public class StockRuleEntity : BaseEntity
    { 
        /// <summary>
        /// 商户
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } 

        public virtual ICollection<StockRuleProdEntity> StockRuleProds { get; set; } = new List<StockRuleProdEntity>();

        public virtual ICollection<StockRuleShopEntity> StockRuleShops { get; set; } = new List<StockRuleShopEntity>();

    }
}
