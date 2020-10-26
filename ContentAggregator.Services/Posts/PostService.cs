using System;
using System.Linq;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Likes;
using ContentAggregator.Repositories.Posts;
using ContentAggregator.Repositories.Tags;
using ContentAggregator.Services.Helpers;
using ContentAggregator.Services.Session;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly ILikeRepository<Post> _likeRepository;
        private readonly ILogger _logger;
        private readonly IPostRepository _postRepository;
        private readonly ISessionService _sessionService;
        private readonly ITagRepository _tagRepository;

        public PostService(
            ISessionService sessionService,
            IPostRepository postRepository,
            ITagRepository tagRepository,
            ILikeRepository<Post> likeRepository,
            ILogger<PostService> logger)
        {
            _sessionService = sessionService;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _likeRepository = likeRepository;
            _logger = logger;
        }

        public async Task<Post> Create(CreatePostDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("create", dto.Content, dto.Title, user);

            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                AuthorId = user.Id,
                Likes = 0,
                Dislikes = 0,
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

        public async Task<Post> Get(string id)
        {
            Post post = await _postRepository.GetById(id);

            if (post == null)
                throw HttpError.NotFound("");

            await UpdatePostWithLikesAndDislikes(post);

            return post;
        }

        public async Task<Post[]> Get()
        {
            Post[] posts = await _postRepository.GetAll();

            foreach (Post post in posts)
                await UpdatePostWithLikesAndDislikes(post);

            return posts;
        }

        public Task<Post[]> Get(int skip, int take) => _postRepository.GetPage(skip, take);

        public async Task Update(string id, UpdatePostDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("modify", dto.Content, dto.Title, user);

            Post post = await _postRepository.GetById(id);
            if (post == null)
            {
                _logger.LogWarning($"Post {id} does not exist");
                throw HttpError.NotFound($"Post {id} does not exist");
            }

            if (post.AuthorId != user.Id)
            {
                _logger.LogWarning($"Post {id} does not belong to user");
                throw HttpError.Forbidden($"Post {id} does not belong to user");
            }

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.LastUpdateTime = DateTime.Now;
            post.Tags = TagHelpers.GetTagsFromText(dto.Content);

            bool success = await _postRepository.Update(post);

            if (!success)
            {
                _logger.LogWarning("Error during update post");
                throw HttpError.InternalServerError("");
            }

            await _tagRepository.Create(post.Tags.Select(x => new Tag
            {
                Name = x,
                PostsNumber = 1
            }).ToArray());
            _logger.LogInformation("Tags have been added");
        }

        public async Task Delete(string id)
        {
            User user = await _sessionService.GetUser();

            Post post = await _postRepository.GetById(id);
            if (post == null)
            {
                _logger.LogWarning($"Post {id} does not exist");
                throw HttpError.NotFound($"Post {id} does not exist");
            }

            if (post.AuthorId != user.Id)
            {
                _logger.LogWarning($"Post {id} does not belong to user");
                throw HttpError.Forbidden($"Post {id} does not belong to user");
            }

            await _postRepository.Delete(id);
        }

        public async Task Rate(string id, RateDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("rate", user);
            Post post = await _postRepository.GetById(id);

            if (post == null)
            {
                _logger.LogWarning($"There is no post with id {id}");
                throw HttpError.NotFound($"There is no post with id {id}");
            }

            await _likeRepository.GiveLike(user, post, dto.IsLike);
        }

        public async Task CancelRate(string id)
        {
            User user = await _sessionService.GetUser();
            Validate("cancel rate", user);

            await _likeRepository.CancelLikeOrDislike(user.Id, id);
        }

        private void Validate(string operationName, string content, string title, User user)
        {
            Validate(operationName, user);

            if (content.Length > Consts.PostContentLength)
                throw HttpError.BadRequest("Content is too long");

            if (title.Length > Consts.PostTitleLength)
                throw HttpError.BadRequest("Title is too long");
        }

        private void Validate(string operationName, User user)
        {
            if (user != null)
                return;

            _logger.LogWarning($"You cannot {operationName} post if you are not logged in");
            throw HttpError.Unauthorized($"You cannot {operationName} post if you are not logged in");
        }

        private async Task UpdatePostWithLikesAndDislikes(Post post)
        {
            post.Likes = await _likeRepository.GetNumberOfLikes(post.Id);
            post.Dislikes = await _likeRepository.GetNumberOfDislikes(post.Id);
        }
    }
}