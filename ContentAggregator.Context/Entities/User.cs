using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContentAggregator.Common;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models;

namespace ContentAggregator.Context.Entities
{
    public class User : BaseEntity
    {
        private static readonly char delimiter = ';';

        [Required]
        [MaxLength(Consts.UsernameMaxLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public CredentialLevel CredentialLevel { get; set; }

        [MaxLength(Consts.DescriptionMaxLength)]
        public string Description { get; set; }

        [NotMapped]
        public string[] BlackListedTags {
            get { return StringBlackListedTags.Split(delimiter); }
            set
            {
                StringBlackListedTags = string.Join($"{delimiter}", value);
            }
        }

        [NotMapped]
        public string[] FollowedTags {
            get { return StringFollowedTags.Split(delimiter); }
            set
            {
                StringFollowedTags = string.Join($"{delimiter}", value);
            }
        }

        [NotMapped]
        public string[] BlackListedUserIds {
            get { return StringBlackListedUserIds.Split(delimiter); }
            set
            {
                StringBlackListedUserIds = string.Join($"{delimiter}", value);
            }
        }

        [NotMapped]
        public string[] FollowedUserIds {
            get { return StringFollowedUserIds.Split(delimiter); }
            set
            {
                StringFollowedUserIds = string.Join($"{delimiter}", value);
            }
        }

        public string StringBlackListedTags { get; set; }
        public string StringFollowedTags { get; set; }
        public string StringBlackListedUserIds { get; set; }
        public string StringFollowedUserIds { get; set; }

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