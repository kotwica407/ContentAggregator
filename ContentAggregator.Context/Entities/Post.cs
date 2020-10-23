using System;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;

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
    }
}