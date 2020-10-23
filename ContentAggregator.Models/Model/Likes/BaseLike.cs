namespace ContentAggregator.Models.Model.Likes
{
    public abstract class BaseLike
    {
        public string EntityId { get; set; }
        public string UserId { get; set; }
        public bool IsLike { get; set; }
    }
}