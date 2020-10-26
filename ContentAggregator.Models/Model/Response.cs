namespace ContentAggregator.Models.Model
{
    public class Response : PostBase
    {
        public string CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}