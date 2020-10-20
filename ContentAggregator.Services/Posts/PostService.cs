using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories;
using ContentAggregator.Repositories.Tags;
using ContentAggregator.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly ICrudRepository<Post> _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICrudRepository<User> _userRepository;

        public PostService(
            ICrudRepository<Post> postRepository,
            ICrudRepository<User> userRepository,
            ITagRepository tagRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PostService> logger)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Post> Create(CreatePostDto dto)
        {
            #region Validate

            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("You cannot create post if you are not logged in");
                throw HttpError.Unauthorized("You cannot create post if you are not logged in");
            }

            if (dto.Content.Length > Consts.PostContentLength)
                throw HttpError.BadRequest("Content is too long");

            if (dto.Title.Length > Consts.PostTitleLength)
                throw HttpError.BadRequest("Title is too long");

            #endregion

            string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            User user = (await _userRepository.Find(x => x.Name == userName)).SingleOrDefault();
            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                AuthorId = user.Id,
                Rate = 0,
                Tags = TagHelpers.GetTagsFromText(dto.Content),
                Id = Guid.NewGuid().ToString()
            };

            await _postRepository.Create(post);
            _logger.LogInformation($"Post {post.Id} has been created");

            await _tagRepository.Create(post.Tags.Select(x => new Tag
            {
                Name = x,
                PostsNumber = 1
            }).ToArray());
            _logger.LogInformation("Tags have been added");

            return post;
        }

        public Task<Post> Get(string id) => _postRepository.GetById(id);
    }
}