using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Services.Session
{
    public interface ISessionService
    {
        Task<User> GetUser();
    }
}