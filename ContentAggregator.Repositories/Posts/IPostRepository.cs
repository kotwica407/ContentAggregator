using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Posts
{
    public interface IPostRepository
    {
        Task<Post[]> GetAll();
        Task<Post> GetById(string id);
        Task<Post[]> GetByAuthor(string authorId);
        Task<Post[]> GetByTag(string tagName);
        Task Create(Post post);
        Task<bool> Update(Post post);
        Task Delete(string id);
        Task RateUp(string id);
        Task RateDown(string id);
    }
}