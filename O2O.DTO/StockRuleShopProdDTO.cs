using System;
using System.Collections.Generic;

namespace O2O.DTO
{
    public class StockRuleShopProdDTO : BaseDTO
    {
        /// <summary>
        /// це╣Й
        /// </summary>
        public string ShopNo { get; set; }

        public virtual List<StockRuleProdDTO> StockRuleProds { get; set; } = new List<StockRuleProdDTO>();
    }
}