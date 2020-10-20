namespace ContentAggregator.Context.Entities
{
    public class Picture : BaseEntity
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] File { get; set; }   
    }
}