using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;
using ContentAggregator.Context.Entities.Likes;

namespace ContentAggregator.Context.Entities
{
    public class Post : PostBaseEntity
    {
        [Required]
        [MaxLength(Consts.PostTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(Consts.PostContentLength)]
        public string Content { get; set; }

        public string[] Tags { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<BaseLikeEntity<Post>> PostLikes { get; set; }
    }
}