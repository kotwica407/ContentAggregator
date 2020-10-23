using System.Net;
using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Responses;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ContentAggregator.Web.Controllers
{
    [ApiController]
    [Route("api/post/{postId}/comment/{commentId}/[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromRoute] string postId, string commentId, [FromBody] CreateResponseDto dto)
        {
            Response response = await _responseService.Create(postId, commentId, dto);
            return CreatedAtAction(nameof(Get), new { postId, commentId, id = response.Id }, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string commentId, string id)
        {
            Response response = await _responseService.Get(commentId, id);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Comment[]), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string commentId)
        {
            Response[] responses = await _responseService.Get(commentId);
            return Ok(responses);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromRoute] string commentId, string id, [FromBody] UpdateResponseDto dto)
        {
            await _responseService.Update(commentId, id, dto);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string commentId, string id)
        {
            await _responseService.Delete(commentId, id);
            return NoContent();
        }


        [HttpPost]
        [Route("rate/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromRoute] string id, [FromBody] RateDto dto)
        {
            await _responseService.Rate(id, dto);
            return NoContent();
        }

        [HttpDelete]
        [Route("rate/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteRate([FromRoute] string id)
        {
            await _responseService.CancelRate(id);
            return NoContent();
        }
    }
}