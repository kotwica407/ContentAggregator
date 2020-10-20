using System.Threading.Tasks;
using ContentAggregator.Models.Dtos.Comments;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Services.Comments
{
    public interface ICommentService
    {
        Task<Comment> Create(string postId, CreateCommentDto dto);
        Task<Comment> Get(string postId, string id);
        Task<Comment[]> Get(string postId);
        Task Update(string postId, string id, UpdateCommentDto dto);
        Task Delete(string postId, string id);
    }
}