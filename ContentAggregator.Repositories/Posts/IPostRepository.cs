using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Posts
{
    public interface IPostRepository : ICrudRepository<Post>
    {
        Task<Post[]> GetPage(int skip, int take);
    }
}