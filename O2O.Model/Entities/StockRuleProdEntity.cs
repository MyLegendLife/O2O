using System;

namespace O2O.Model.Entities
{
    public class StockRuleProdEntity : BaseEntity
    { 
        /// <summary>
        /// ��Ʒ���
        /// </summary>
        public string ProdNo { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// ���Ű�ȫ���
        /// </summary>
        public double MtStock { get; set; }

        /// <summary>
        /// ����ô��ȫ���
        /// </summary>
        public double EleStock { get; set; }

        public Guid StockRuleId { get; set; }
        public virtual StockRuleEntity StockRule { get; set; }
    }
}
