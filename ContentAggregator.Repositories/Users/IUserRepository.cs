using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Users
{
    public interface IUserRepository : ICrudRepository<User>
    {
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string emailAddress);
    }
}