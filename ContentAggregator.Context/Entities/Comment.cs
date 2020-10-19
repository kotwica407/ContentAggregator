using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;

namespace ContentAggregator.Context.Entities
{
    public class Comment : BaseEntity
    {
        [Required]
        public string AuthorId { get; set; }

        [Required]
        [MaxLength(Consts.CommentContentLength)]
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        [Required]
        public Post Post { get; set; }

        public List<Response> Responses { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}