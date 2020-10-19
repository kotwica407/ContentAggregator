using System;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;

namespace ContentAggregator.Context.Entities
{
    public class Response : BaseEntity
    {
        [Required]
        public string AuthorId { get; set; }

        [Required]
        [MaxLength(Consts.CommentContentLength)]
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        [Required]
        public Comment Comment { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}