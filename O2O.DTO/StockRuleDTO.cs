namespace O2O.DTO
{
    public class StockRuleDTO : BaseDTO
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
    }
}