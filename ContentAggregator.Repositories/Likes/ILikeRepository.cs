using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Likes
{
    public interface ILikeRepository<T> where T : PostBase
    {
        Task GiveLike(User user, T postBase, bool isLike);
        Task CancelLikeOrDislike(string userId, string postBaseId);
        Task<int> GetNumberOfLikes(string postBaseId);
        Task<int> GetNumberOfDislikes(string postBaseId);
    }
}