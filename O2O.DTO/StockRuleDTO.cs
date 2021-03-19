namespace O2O.DTO
{
    public class StockRuleDTO : BaseDTO
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
    }
}