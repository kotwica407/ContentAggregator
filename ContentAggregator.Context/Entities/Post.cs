using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;

namespace ContentAggregator.Context.Entities
{
    public class Post
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [MaxLength(Consts.PostTitleLength)]
        public string Title { get; set; }
        [Required]
        [MaxLength(Consts.PostContentLength)]
        public string Content { get; set; }
        public string[] Tags { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public List<Comment> Comments { get; set; }
        [Required]
        public int Rate { get; set; }
    }
}