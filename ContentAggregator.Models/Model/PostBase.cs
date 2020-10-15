using System;

namespace ContentAggregator.Models.Model
{
    public abstract class PostBase
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int Rate { get; set; }
    }
}