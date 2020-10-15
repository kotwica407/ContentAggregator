namespace ContentAggregator.Models.Model
{
    public class Comment : PostBase
    {
        public Post Post { get; set; }
        public Response[] Responses { get; set; }
    }
}