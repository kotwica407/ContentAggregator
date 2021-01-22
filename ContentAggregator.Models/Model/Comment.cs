namespace ContentAggregator.Models.Model
{
    public class Comment : PostBase
    {
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}