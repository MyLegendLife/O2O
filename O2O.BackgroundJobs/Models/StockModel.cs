namespace O2O.BackgroundJobs.Models
{
    public class StockModel
    {
        public string ProdCode { get; set; }  //美团使用

        public long ProdId { get; set; }  //饿了么使用

        public long SpecId { get; set; } //饿了么使用

        public string ProdNo { get; set; }

        public double CurrentStock { get; set; }

        public double MtMarkStock { get; set; }

        public double EleMarkStock { get; set; }
    }
}