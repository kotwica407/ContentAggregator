using System;
using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities
{
    public abstract class PostBaseEntity : BaseEntity
    {
        [Required]
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        [Required]
        public int Likes { get; set; }

        [Required]
        public int Dislikes { get; set; }
    }
}