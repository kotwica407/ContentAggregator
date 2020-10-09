using ContentAggregator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContentAggregator.Context.Entities
{
    public class UserEntity
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public CredentialLevel CredentialLevel { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
    }
}
