using System.Threading.Tasks;
using ContentAggregator.Models.Model.Likes;

namespace ContentAggregator.Repositories.Likes
{
    public interface ILikeRepository<T> where T : BaseLike
    {
        Task GiveLike(T like);
        Task CancelLikeOrDislike(string entityId, string userId);
        Task<int> GetNumberOfLikes(string entityId);
        Task<int> GetNumberOfDislikes(string entityId);
    }
}