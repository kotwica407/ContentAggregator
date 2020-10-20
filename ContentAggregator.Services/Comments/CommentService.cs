using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos.Comments;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICrudRepository<Comment> _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly ICrudRepository<Post> _postRepository;
        private readonly ICrudRepository<User> _userRepository;

        public CommentService(
            ICrudRepository<Comment> commentRepository,
            ICrudRepository<Post> postRepository,
            ICrudRepository<User> userRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Comment> Create(string postId, CreateCommentDto dto)
        {
            Validate("create", dto.Content);

            string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            User user = (await _userRepository.Find(x => x.Name == userName)).SingleOrDefault();
            Post post = await _postRepository.GetById(postId);
            if (post == null)
            {
                _logger.LogWarning($"There is no post {postId}");
                throw HttpError.NotFound($"There is no post {postId}");
            }

            var comment = new Comment
            {
                Content = dto.Content,
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                AuthorId = user.Id,
                Rate = 0,
                PostId = postId,
                Id = Guid.NewGuid().ToString()
            };

            await _commentRepository.Create(comment);
            _logger.LogInformation($"Comment {comment.Id} has been created");

            return comment;
        }

        public async Task<Comment> Get(string postId, string id)
        {
            Comment comment = await _commentRepository.GetById(id);

            if (comment == null)
                throw HttpError.NotFound("");

            if (comment.PostId != postId)
                throw HttpError.NotFound("");

            return comment;
        }

        public Task<Comment[]> Get(string postId) => _commentRepository.Find(x => x.PostId == postId);

        public async Task Update(string postId, string id, UpdateCommentDto dto)
        {
            Validate("modify", dto.Content);

            string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            User user = (await _userRepository.Find(x => x.Name == userName)).SingleOrDefault();

            Comment comment = await _commentRepository.GetById(id);
            if (comment == null)
            {
                _logger.LogWarning($"Post {id} does not exist");
                throw HttpError.NotFound($"Post {id} does not exist");
            }

            if (comment.PostId != postId)
                throw HttpError.NotFound("");

            if (comment.AuthorId != user.Id)
            {
                _logger.LogWarning($"Post {id} does not belong to user");
                throw HttpError.Forbidden($"Post {id} does not belong to user");
            }

            comment.Content = dto.Content;
            comment.LastUpdateTime = DateTime.Now;

            bool success = await _commentRepository.Update(comment);

            if (!success)
            {
                _logger.LogWarning("Error during update comment");
                throw HttpError.InternalServerError("");
            }
        }

        public async Task Delete(string postId, string id)
        {
            string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            User user = (await _userRepository.Find(x => x.Name == userName)).SingleOrDefault();

            Comment comment = await _commentRepository.GetById(id);
            if (comment == null)
            {
                _logger.LogWarning($"Comment {id} does not exist");
                throw HttpError.NotFound($"Comment {id} does not exist");
            }

            if (comment.PostId != postId)
                throw HttpError.NotFound("");

            if (comment.AuthorId != user.Id)
            {
                _logger.LogWarning($"Post {id} does not belong to user");
                throw HttpError.Forbidden($"Post {id} does not belong to user");
            }

            await _commentRepository.Delete(id);
        }

        private void Validate(string operationName, string content)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                _logger.LogWarning($"You cannot {operationName} post if you are not logged in");
                throw HttpError.Unauthorized($"You cannot {operationName} post if you are not logged in");
            }

            if (content.Length > Consts.PostContentLength)
                throw HttpError.BadRequest("Content is too long");
        }
    }
}