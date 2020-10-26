using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;
using ContentAggregator.Context.Entities.Likes;

namespace ContentAggregator.Context.Entities
{
    public class Response : PostBaseEntity
    {
        [Required]
        [MaxLength(Consts.CommentContentLength)]
        public string Content { get; set; }

        [Required]
        public string CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual ICollection<BaseLikeEntity<Response>> ResponseLikes { get; set; }
    }
}