using System;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Comments;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Comments;
using ContentAggregator.Repositories.Likes;
using ContentAggregator.Repositories.Posts;
using ContentAggregator.Services.Session;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository<Comment> _likeRepository;
        private readonly ILogger _logger;
        private readonly IPostRepository _postRepository;
        private readonly ISessionService _sessionService;

        public CommentService(
            ICommentRepository commentRepository,
            IPostRepository postRepository,
            ILogger<CommentService> logger,
            ISessionService sessionService,
            ILikeRepository<Comment> likeRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _logger = logger;
            _sessionService = sessionService;
            _likeRepository = likeRepository;
        }

        public async Task<Comment> Create(string postId, CreateCommentDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("create", dto.Content, user);

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
                Likes = 0,
                Dislikes = 0,
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

            await UpdateCommentWithLikesAndDislikes(comment);

            return comment;
        }

        public async Task<Comment[]> Get(string postId)
        {
            Comment[] comments = await _commentRepository.Find(x => x.PostId == postId);
            foreach (Comment comment in comments)
                await UpdateCommentWithLikesAndDislikes(comment);

            return comments;
        }

        public async Task Update(string postId, string id, UpdateCommentDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("modify", dto.Content, user);

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
                _logger.LogWarning($"Comment {id} does not belong to user");
                throw HttpError.Forbidden($"Comment {id} does not belong to user");
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
            User user = await _sessionService.GetUser();
            if (user == null)
            {
                _logger.LogWarning("You cannot delete comment if you are not logged in");
                throw HttpError.Unauthorized("You cannot delete comment if you are not logged in");
            }

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
                _logger.LogWarning($"Comment {id} does not belong to user");
                throw HttpError.Forbidden($"Comment {id} does not belong to user");
            }

            await _commentRepository.Delete(id);
        }

        public async Task Rate(string id, RateDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("rate", user);
            Comment comment = await _commentRepository.GetById(id);

            if (comment == null)
            {
                _logger.LogWarning($"There is no comment with id {id}");
                throw HttpError.NotFound($"There is no comment with id {id}");
            }

            await _likeRepository.GiveLike(user, comment, dto.IsLike);
        }

        public async Task CancelRate(string id)
        {
            User user = await _sessionService.GetUser();
            Validate("cancel rate", user);

            await _likeRepository.CancelLikeOrDislike(id, user.Id);
        }

        private void Validate(string operationName, string content, User user)
        {
            Validate(operationName, user);

            if (content.Length > Consts.PostContentLength)
                throw HttpError.BadRequest("Content is too long");
        }

        private void Validate(string operationName, User user)
        {
            if (user != null)
                return;

            _logger.LogWarning($"You cannot {operationName} comment if you are not logged in");
            throw HttpError.Unauthorized($"You cannot {operationName} comment if you are not logged in");
        }

        private async Task UpdateCommentWithLikesAndDislikes(Comment comment)
        {
            comment.Likes = await _likeRepository.GetNumberOfLikes(comment.Id);
            comment.Dislikes = await _likeRepository.GetNumberOfDislikes(comment.Id);
        }
    }
}