using System.ComponentModel.DataAnnotations;

namespace ContentAggregator.Context.Entities
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        public int PostsNumber { get; set; }
    }
}