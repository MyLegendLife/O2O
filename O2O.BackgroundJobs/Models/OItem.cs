using System.Collections.Generic;

namespace O2O.BackgroundJobs.Models
{
    public enum OItemWeekEnum
    {
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATURDAY,
        SUNDAY
    }

    public class Labels
    {
        /// <summary>
        /// 
        /// </summary>
        public int isFeatured { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isGum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isNew { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isSpicy { get; set; }
    }

    public class OSupplyLinkSpec
    {
        /// <summary>
        /// 度
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> value { get; set; }
    }

    public class OSupplyLink
    {
        /// <summary>
        /// 代表冷链
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OSupplyLinkSpec> minorSpec { get; set; }
    }

    public class OSpec
    {
        /// <summary>
        /// 
        /// </summary>
        public long specId { get; set; }
        /// <summary>
        /// 大份
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int stock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int maxStock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int stockStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double packingFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int onShelf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string extendCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string barCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int activityLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OSupplyLink supplyLink { get; set; }
    }

    public class OItemTime
    {
        /// <summary>
        /// 
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
    }

    public class OItemSellingTime
    {
        /// <summary>
        /// 
        /// </summary>
        public List<OItemWeekEnum> weeks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OItemTime> times { get; set; }
    }

    public class OItemAttribute
    {
        /// <summary>
        /// 甜度
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> details { get; set; }
    }

    public class OMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 鸡蛋
        /// </summary>
        public string name { get; set; }
    }

    public class OItem
    {
        /// <summary>
        /// 香脆可口，外焦里嫩
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 白切鸡
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isValid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int recentPopularity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long categoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long shopId { get; set; }
        /// <summary>
        /// 烤鸡大王
        /// </summary>
        public string shopName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Labels labels { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OSpec> specs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OItemSellingTime sellingTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OItemAttribute> attributes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long backCategoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int minPurchaseQuantity { get; set; }
        /// <summary>
        /// 份
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int setMeal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OMaterial> materials { get; set; }
    }
}
