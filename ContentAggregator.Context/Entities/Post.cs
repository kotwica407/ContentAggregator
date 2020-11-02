using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContentAggregator.Common;
using ContentAggregator.Context.Entities.Likes;

namespace ContentAggregator.Context.Entities
{
    public class Post : PostBaseEntity
    {
        private string _tags;
        private static readonly char delimiter = ';';

        [Required]
        [MaxLength(Consts.PostTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(Consts.PostContentLength)]
        public string Content { get; set; }

        [NotMapped]
        public string[] Tags
        {
            get => _tags.Split(delimiter);
            set => _tags = string.Join($"{delimiter}", value);
        }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<BaseLikeEntity<Post>> PostLikes { get; set; }
    }
}