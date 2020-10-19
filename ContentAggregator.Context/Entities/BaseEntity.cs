using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public string Id { get; set; }
    }
}