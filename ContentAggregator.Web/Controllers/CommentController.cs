using System.Net;
using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Comments;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Comments;
using Microsoft.AspNetCore.Mvc;

namespace ContentAggregator.Web.Controllers
{
    [ApiController]
    [Route("api/post/{postId}/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Comment), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromRoute] string postId, [FromBody] CreateCommentDto dto)
        {
            Comment comment = await _commentService.Create(postId, dto);
            return CreatedAtAction(nameof(Get), new { postId, id = comment.Id}, comment);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Comment), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string postId, string id)
        {
            Comment comment = await _commentService.Get(postId, id);
            return Ok(comment);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Comment[]), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string postId)
        {
            Comment[] comments = await _commentService.Get(postId);
            return Ok(comments);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Forbidden)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromRoute] string postId, string id, [FromBody] UpdateCommentDto dto)
        {
            await _commentService.Update(postId, id, dto);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.Forbidden)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string postId, string id)
        {
            await _commentService.Delete(postId, id);
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
            await _commentService.Rate(id, dto);
            return NoContent();
        }

        [HttpDelete]
        [Route("rate/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteRate([FromRoute] string id)
        {
            await _commentService.CancelRate(id);
            return NoContent();
        }
    }
}