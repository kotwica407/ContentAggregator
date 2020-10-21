using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Users
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<User[]> GetAll();
        Task<User> GetById(string id);
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string emailAddress);
        Task<User[]> Find(Expression<Func<User, bool>> predicate);
        Task<bool> Update(User user);
        Task Delete(string userId);
    }
}
