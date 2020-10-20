namespace ContentAggregator.Models.Model
{
    public class Picture : BaseModel
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] File { get; set; }
    }
}