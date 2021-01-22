namespace ContentAggregator.Context.Entities
{
    public class Picture : BaseEntity
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] File { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}