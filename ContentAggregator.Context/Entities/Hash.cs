using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities
{
    public class Hash : BaseEntity
    {
        [Required]
        public string PasswordHash { get; set; }
    }
}