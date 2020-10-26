using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models;

namespace ContentAggregator.Context.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(Consts.UsernameMaxLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public CredentialLevel CredentialLevel { get; set; }

        [MaxLength(Consts.DescriptionMaxLength)]
        public string Description { get; set; }

        public string[] BlackListedTags { get; set; }

        public string[] FollowedTags { get; set; }

        public string[] BlackListedUserIds { get; set; }

        public string[] FollowedUserIds { get; set; }
        public string PictureId { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ICollection<BaseLikeEntity<Post>> PostLikes { get; set; }
        public virtual ICollection<BaseLikeEntity<Comment>> CommentLikes { get; set; }
        public virtual ICollection<BaseLikeEntity<Response>> ResponseLikes { get; set; }
    }
}