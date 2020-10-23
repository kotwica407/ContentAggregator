using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Responses;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Services.Responses
{
    public interface IResponseService
    {
        Task<Response> Create(string postId, string commentId, CreateResponseDto dto);
        Task<Response> Get(string commentId, string id);
        Task<Response[]> Get(string commentId);
        Task Update(string commentId, string id, UpdateResponseDto dto);
        Task Delete(string commentId, string id);
        Task Rate(string id, RateDto dto);
        Task CancelRate(string id);
    }
}