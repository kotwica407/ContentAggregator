namespace ContentAggregator.Models.Model
{
    public class Post : PostBase
    {
        public Tag[] Tags { get; set; }
        public Comment[] Comments { get; set; }
    }
}