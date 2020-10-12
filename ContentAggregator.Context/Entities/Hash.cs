using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities
{
    public class Hash
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}