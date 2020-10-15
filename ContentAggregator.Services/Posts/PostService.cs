using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Posts;
using ContentAggregator.Repositories.Tags;
using ContentAggregator.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;

        public PostService(
            IPostRepository postRepository,
            IUserRepository userRepository,
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

            if(!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
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
            User user = await _userRepository.GetByUserName(userName);
            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                Comments = new Comment[0],
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                AuthorId = user.Id,
                Rate = 0,
                Tags = GetTagsFromText(dto.Content),
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

        private static string[] GetTagsFromText(string text)
        {
            List<string> result = new List<string>();
            string[] parts = text.Split(null); //white-space is assumed to be the splitting character
            foreach (string part in parts)
            {
                int firstPositionOfHash = part.IndexOf('#');
                switch (firstPositionOfHash)
                {
                    case -1:
                        continue;
                    default:
                        result.AddRange(part.Split('#').Skip(1));
                        break;
                }
            }

            return result.Distinct().ToArray();
        }
    }
}