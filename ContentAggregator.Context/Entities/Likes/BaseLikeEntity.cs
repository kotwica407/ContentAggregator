using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities.Likes
{
    public abstract class BaseLikeEntity
    {
        [Key]
        public string UserId { get; set; }

        [Key]
        public string EntityId { get; set; }

        [Required]
        public bool IsLike { get; set; }
    }
}