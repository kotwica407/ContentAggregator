using System;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Responses;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Models.Model.Likes;
using ContentAggregator.Repositories.Comments;
using ContentAggregator.Repositories.Likes;
using ContentAggregator.Repositories.Responses;
using ContentAggregator.Services.Session;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Responses
{
    public class ResponseService : IResponseService
    {
        private readonly ILogger _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly ISessionService _sessionService;
        private readonly ILikeRepository<ResponseLike> _likeRepository;

        public ResponseService(
            IResponseRepository responseRepository,
            ICommentRepository commentRepository,
            ILogger<ResponseService> logger,
            ISessionService sessionService,
            ILikeRepository<ResponseLike> likeRepository)
        {
            _responseRepository = responseRepository;
            _commentRepository = commentRepository;
            _logger = logger;
            _sessionService = sessionService;
            _likeRepository = likeRepository;
        }

        public async Task<Response> Create(string postId, string commentId, CreateResponseDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("create", dto.Content, user);

            Comment comment = await _commentRepository.GetById(commentId);
            if (comment == null)
            {
                _logger.LogWarning($"There is no comment {postId}");
                throw HttpError.NotFound($"There is no comment {postId}");
            }

            if (comment.PostId != postId)
            {
                _logger.LogWarning($"Comment {commentId} does not belong to post {postId}");
                throw HttpError.BadRequest($"Comment does not belong to post");
            }

            var response = new Response
            {
                Content = dto.Content,
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                AuthorId = user.Id,
                Likes = 0,
                Dislikes = 0,
                CommentId = commentId,
                Id = Guid.NewGuid().ToString()
            };

            await _responseRepository.Create(response);
            _logger.LogInformation($"Response {response.Id} has been created");

            return response;
        }

        public async Task<Response> Get(string commentId, string id)
        {
            Response response = await _responseRepository.GetById(id);

            if (response == null)
                throw HttpError.NotFound("");

            if (response.CommentId != commentId)
                throw HttpError.NotFound("");

            return response;
        }

        public Task<Response[]> Get(string commentId) => _responseRepository.Find(x => x.CommentId == commentId);

        public async Task Update(string commentId, string id, UpdateResponseDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("modify", dto.Content, user);

            Response response = await _responseRepository.GetById(id);
            if (response == null)
            {
                _logger.LogWarning($"Response {id} does not exist");
                throw HttpError.NotFound($"Response {id} does not exist");
            }

            if (response.CommentId != commentId)
                throw HttpError.NotFound("");

            if (response.AuthorId != user.Id)
            {
                _logger.LogWarning($"Response {id} does not belong to user");
                throw HttpError.Forbidden($"Response {id} does not belong to user");
            }

            response.Content = dto.Content;
            response.LastUpdateTime = DateTime.Now;

            bool success = await _responseRepository.Update(response);

            if (!success)
            {
                _logger.LogWarning("Error during update response");
                throw HttpError.InternalServerError("");
            }
        }

        public async Task Delete(string commentId, string id)
        {
            User user = await _sessionService.GetUser();
            if (user == null)
            {
                _logger.LogWarning("You cannot delete comment if you are not logged in");
                throw HttpError.Unauthorized("You cannot delete comment if you are not logged in");
            }

            Response response = await _responseRepository.GetById(id);
            if (response == null)
            {
                _logger.LogWarning($"Response {id} does not exist");
                throw HttpError.NotFound($"Response {id} does not exist");
            }

            if (response.CommentId != commentId)
                throw HttpError.NotFound("");

            if (response.AuthorId != user.Id)
            {
                _logger.LogWarning($"Response {id} does not belong to user");
                throw HttpError.Forbidden($"Response {id} does not belong to user");
            }

            await _responseRepository.Delete(id);
        }

        public async Task Rate(string id, RateDto dto)
        {
            User user = await _sessionService.GetUser();
            Validate("rate", user);

            await _likeRepository.GiveLike(new ResponseLike
            {
                EntityId = id,
                UserId = user.Id,
                IsLike = dto.IsLike
            });
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

            _logger.LogWarning($"You cannot {operationName} response if you are not logged in");
            throw HttpError.Unauthorized($"You cannot {operationName} response if you are not logged in");
        }
    }
}