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
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Create(User user)
        {
            Context.Entities.User entity = UserMapper.Map(user);
            if (entity == null)
                throw new Exception("User entity cannot be null");
            await _applicationDbContext.Users.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            Context.Entities.User existingEntity = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existingEntity != null)
            {
                _applicationDbContext.Users.Remove(existingEntity);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task<User> GetByEmail(string emailAddress)
        {
            Context.Entities.User entity =
                await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == emailAddress);
            return UserMapper.Map(entity);
        }

        public async Task<User> GetByUserName(string userName)
        {
            Context.Entities.User entity =
                await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userName);
            return UserMapper.Map(entity);
        }

        public async Task<bool> Update(User user)
        {
            Context.Entities.User entity = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (entity == null)
                return false;

            entity.Name = user.Name;
            entity.Email = user.Email;
            entity.Description = user.Description;
            entity.CredentialLevel = user.CredentialLevel;

            try
            {
                return await _applicationDbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}