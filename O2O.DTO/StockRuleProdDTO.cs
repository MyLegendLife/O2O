using System;

namespace O2O.DTO
{
    public class StockRuleProdDTO : BaseDTO
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
    }
}