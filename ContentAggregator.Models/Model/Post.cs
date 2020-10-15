namespace ContentAggregator.Models.Model
{
    public class Post : PostBase
    {
        public string[] Tags { get; set; }
        public Comment[] Comments { get; set; }
    }
}