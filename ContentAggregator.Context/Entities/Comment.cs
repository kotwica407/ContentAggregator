using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;

namespace ContentAggregator.Context.Entities
{
    public class Comment : PostBaseEntity
    {
        [Required]
        [MaxLength(Consts.CommentContentLength)]
        public string Content { get; set; }

        [Required]
        public string PostId { get; set; }
    }
}