using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities.Likes
{
    public class BaseLikeEntity<T> where T : PostBaseEntity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string EntityId { get; set; }
        public virtual T Entity { get; set; }

        [Required]
        public bool IsLike { get; set; }
    }
}