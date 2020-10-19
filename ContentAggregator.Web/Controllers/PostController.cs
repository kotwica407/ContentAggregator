using System.Threading.Tasks;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Posts;
using Microsoft.AspNetCore.Mvc;

namespace ContentAggregator.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
        {
            Post post = await _postService.Create(dto);
            return CreatedAtAction(nameof(Get), new {id = post.Id}, post);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            Post post = await _postService.Get(id);
            return Ok(post);
        }
    }
}