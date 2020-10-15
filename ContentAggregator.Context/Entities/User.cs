using System.ComponentModel.DataAnnotations;
using ContentAggregator.Common;
using ContentAggregator.Models;

namespace ContentAggregator.Context.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }

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
    }
}