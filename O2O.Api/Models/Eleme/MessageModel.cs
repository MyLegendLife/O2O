namespace O2O.Api.Models.Eleme
{
    public class MessageModel
    {
        public string requestId { get; set; }

        public int type { get; set; }

        public string appId { get; set; }

        public string message { get; set; }

        public long shopId { get; set; }

        public long timestamp { get; set; }

        public string signature { get; set; }

        public string userId { get; set; }
    }
}