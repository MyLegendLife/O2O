using System;

namespace O2O.Model.Entities
{
    public class StockRuleProdEntity : BaseEntity
    { 
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProdNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// 美团安全库存
        /// </summary>
        public double MtStock { get; set; }

        /// <summary>
        /// 饿了么安全库存
        /// </summary>
        public double EleStock { get; set; }

        public Guid StockRuleId { get; set; }
        public virtual StockRuleEntity StockRule { get; set; }
    }
}
