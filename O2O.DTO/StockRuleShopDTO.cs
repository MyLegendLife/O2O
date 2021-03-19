using System;

namespace O2O.DTO
{
    public class StockRuleShopDTO : BaseDTO
    {
        /// <summary>
        /// �ŵ�
        /// </summary>
        public string ShopNo { get; set; }

        public string ShopName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public Guid StockRuleId { get; set; }

        public string RuleName { get; set; }
    }
}