using System.Net;
using System.Threading.Tasks;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Posts;
using Microsoft.AspNetCore.Mvc;

namespace ContentAggregator.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Post), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
        {
            Post post = await _postService.Create(dto);
            return CreatedAtAction(nameof(Get), new {id = post.Id}, post);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            Post post = await _postService.Get(id);
            return Ok(post);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(Post[]), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromQuery] int skip, int take)
        {
            Post[] posts = await _postService.Get(skip, take);
            return Ok(posts);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdatePostDto dto)
        {
            await _postService.Update(id, dto);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _postService.Delete(id);
            return NoContent();
        }
    }
}