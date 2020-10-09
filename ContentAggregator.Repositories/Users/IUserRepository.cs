using ContentAggregator.Context.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContentAggregator.Repositories.Users
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<bool> Update(User user);
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string emailAddress);
        Task<User> Get(Expression<Func<User, bool>> predicate);
        Task Delete(string userId);
    }
}
