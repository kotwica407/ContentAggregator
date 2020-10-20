namespace ContentAggregator.Models.Model
{
    public class Post : PostBase
    {
        public string Title { get; set; }
        public string[] Tags { get; set; }
    }
}