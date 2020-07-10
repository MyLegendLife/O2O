namespace O2O.Model
{
    public class Mt_AccountEntity : BaseEntity
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string WaimaiAppId { get; set; }
        public string WaimaiAppSecret { get; set; }
        public string TuangouAppKey { get; set; }
        public string TuangouAppSecret { get; set; }

        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
