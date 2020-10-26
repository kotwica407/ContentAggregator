using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;
using ContentAggregator.Context.Entities.Likes;

namespace ContentAggregator.Context.Entities
{
    public class Comment : PostBaseEntity
    {
        [Required]
        [MaxLength(Consts.CommentContentLength)]
        public string Content { get; set; }

        [Required]
        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public virtual ICollection<Response> Responses { get; set; }
        public virtual ICollection<BaseLikeEntity<Comment>> CommentLikes { get; set; }
    }
}