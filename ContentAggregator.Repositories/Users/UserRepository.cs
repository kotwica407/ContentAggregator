using System;
using System.Threading.Tasks;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

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
            Context.Entities.User entity = UserMapper.Map(user);
            if (entity == null)
                throw new Exception("User entity cannot be null");
            await _userContext.Users.AddAsync(entity);
            await _userContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            Context.Entities.User existingEntity = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existingEntity != null)
            {
                _userContext.Users.Remove(existingEntity);
                await _userContext.SaveChangesAsync();
            }
        }

        public async Task<User> GetByEmail(string emailAddress)
        {
            Context.Entities.User entity =
                await _userContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == emailAddress);
            return UserMapper.Map(entity);
        }

        public async Task<User> GetByUserName(string userName)
        {
            Context.Entities.User entity =
                await _userContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userName);
            return UserMapper.Map(entity);
        }

        public async Task<bool> Update(User user)
        {
            Context.Entities.User entity = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (entity == null)
                return false;

            entity.Name = user.Name;
            entity.Email = user.Email;
            entity.Description = user.Description;
            entity.CredentialLevel = user.CredentialLevel;

            try
            {
                return await _userContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}