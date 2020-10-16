using System.Threading.Tasks;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Services.Posts
{
    public interface IPostService
    {
        Task<Post> Create(CreatePostDto dto);
        Task<Post> Get(string id);
    }
}