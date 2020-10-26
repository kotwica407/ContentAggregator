using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Comments
{
    public class CommentRepository : DbRepository<Comment, Context.Entities.Comment>, ICommentRepository
    {
        public CommentRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
        }
    }
}