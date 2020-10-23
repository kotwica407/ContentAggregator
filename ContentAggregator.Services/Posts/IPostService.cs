using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Services.Posts
{
    public interface IPostService
    {
        Task<Post> Create(CreatePostDto dto);
        Task<Post> Get(string id);
        Task<Post[]> Get();
        Task<Post[]> Get(int skip, int take);
        Task Update(string id, UpdatePostDto dto);
        Task Delete(string id);
        Task Rate(string id, RateDto dto);
        Task CancelRate(string id);
    }
}