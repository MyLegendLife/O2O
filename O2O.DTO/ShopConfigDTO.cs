namespace O2O.DTO
{
    public class ShopConfigDTO 
    {
        public string UserId { get; set; }

        public string ShopNo { get; set; }

        public string ShopName { get; set; }

        public int MtAutoConfirm { get; set; }

        public int EleAutoConfirm { get; set; }

        /// <summary>
        /// 自动生成销售 0 关闭 1开启
        /// </summary>
        public int AutoSale { get; set; }

        /// <summary>
        /// 自动同步库存 0 关闭 1开启
        /// </summary>
        public int MtAutoSync { get; set; }

        /// <summary>
        /// 自动同步库存 0 关闭 1开启
        /// </summary>
        public int EleAutoSync { get; set; }
    }
}
