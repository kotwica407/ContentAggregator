using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContentAggregator.Context.Entities
{
    public class HashEntity
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
