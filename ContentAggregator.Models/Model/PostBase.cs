using System;

namespace ContentAggregator.Models.Model
{
    public abstract class PostBase : BaseModel
    {
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}