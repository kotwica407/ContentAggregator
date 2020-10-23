using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;

namespace ContentAggregator.Context.Entities
{
    public class Response : PostBaseEntity
    {
        [Required]
        [MaxLength(Consts.CommentContentLength)]
        public string Content { get; set; }

        [Required]
        public string CommentId { get; set; }
    }
}