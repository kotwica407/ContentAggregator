using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

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
    }
}