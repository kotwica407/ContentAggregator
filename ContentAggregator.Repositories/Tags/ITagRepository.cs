using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Tags
{
    public interface ITagRepository
    {
        Task<Tag[]> GetAll();
        Task<Tag> GetByName(string tagName);
        Task Create(Tag tag);
        Task Create(Tag[] tags);
        Task Delete(string tagName);
    }
}