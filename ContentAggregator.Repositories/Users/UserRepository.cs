using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;
using Picture = ContentAggregator.Context.Entities.Picture;

namespace ContentAggregator.Repositories.Users
{
    public class UserRepository : DbRepository<User, Context.Entities.User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
            _context = context;
        }

        public async Task<User> GetByEmail(string emailAddress)
        {
            Context.Entities.User entity =
                await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == emailAddress);
            return UserMapper.Map(entity);
        }

        public async Task<User> GetByUserName(string userName)
        {
            Context.Entities.User entity =
                await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userName);
            return UserMapper.Map(entity);
        }

        public override async Task Delete(string id)
        {
            Picture existingPictureEntity = await _context.Pictures.FirstOrDefaultAsync(x => x.UserId == id);
            if (existingPictureEntity != null)
            {
                _context.Pictures.Remove(existingPictureEntity);
                await _context.SaveChangesAsync();
            }

            await base.Delete(id);
        }
    }
}