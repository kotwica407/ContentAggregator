using ContentAggregator.Context;
using ContentAggregator.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContentAggregator.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task Create(User user)
        {
            await _userContext.Users.AddAsync(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            var existingEntity = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existingEntity != null)
            {
                _userContext.Users.Remove(existingEntity);
                await _userContext.SaveChangesAsync();
            }  
        }

        public async Task<User> Get(Expression<Func<User, bool>> predicate)
        {
            return await _userContext.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<User> GetByEmail(string emailAddress)
        {
            return await _userContext.Users.FirstOrDefaultAsync(x => x.Email == emailAddress);
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await _userContext.Users.FirstOrDefaultAsync(x => x.Name == userName);
        }

        public async Task<bool> Update(User user)
        {
            var existingEntity = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (existingEntity == null)
                return false;

            existingEntity.Name = user.Name;
            existingEntity.Email = user.Email;
            existingEntity.Description = user.Description;
            existingEntity.CredentialLevel = user.CredentialLevel;

            await _userContext.SaveChangesAsync();
            return true;
        }
    }
}
