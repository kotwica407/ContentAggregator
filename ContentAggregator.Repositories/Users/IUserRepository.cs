using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Users
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<bool> Update(User user);
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string emailAddress);
        Task Delete(string userId);
    }
}
