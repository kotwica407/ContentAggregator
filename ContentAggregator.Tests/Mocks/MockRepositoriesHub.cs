using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using ContentAggregator.Context.Entities;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Comments;
using ContentAggregator.Repositories.Hashes;
using ContentAggregator.Repositories.Likes;
using ContentAggregator.Repositories.Pictures;
using ContentAggregator.Repositories.Posts;
using ContentAggregator.Repositories.Responses;
using ContentAggregator.Repositories.Tags;
using ContentAggregator.Repositories.Users;
using Moq;
using Comment = ContentAggregator.Models.Model.Comment;
using Hash = ContentAggregator.Models.Model.Hash;
using Picture = ContentAggregator.Models.Model.Picture;
using Post = ContentAggregator.Models.Model.Post;
using Response = ContentAggregator.Models.Model.Response;
using Tag = ContentAggregator.Models.Model.Tag;
using User = ContentAggregator.Models.Model.User;

namespace ContentAggregator.UnitTests.Mocks
{
    internal class MockRepositoriesHub
    {
        private readonly List<User> _users;
        private readonly List<Hash> _hashes;
        private readonly List<Picture> _pictures;
        private readonly List<Post> _posts;
        private readonly List<Comment> _comments;
        private readonly List<Response> _responses;
        private readonly List<Tag> _tags;
        private readonly List<BaseLikeEntity<Context.Entities.Post>> _postLikes;
        private readonly List<BaseLikeEntity<Context.Entities.Comment>> _commentLikes;
        private readonly List<BaseLikeEntity<Context.Entities.Response>> _responseLikes;

        private void SetupUserRepositoryMock()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            UserRepositoryMock.Setup(m => m.GetByUserName(It.IsAny<string>()))
               .Returns<string>(name => Task.FromResult(MakeCopy(_users.SingleOrDefault(u => u.Name == name))));
            UserRepositoryMock.Setup(m => m.GetByEmail(It.IsAny<string>()))
               .Returns<string>(email => Task.FromResult(MakeCopy(_users.SingleOrDefault(u => u.Email == email))));
            UserRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<User, bool>>>()))
               .Returns<Expression<Func<User, bool>>>(predicate =>
                    Task.FromResult(_users.Where(predicate.Compile().Invoke).Select(MakeCopy).ToArray()));
            UserRepositoryMock.Setup(m => m.GetById(It.IsAny<string>()))
               .Returns<string>(id => Task.FromResult(MakeCopy(_users.SingleOrDefault(u => u.Id == id))));
            UserRepositoryMock.Setup(m => m.GetAll())
               .Returns(() => Task.FromResult(_users.Select(MakeCopy).ToArray()));
            UserRepositoryMock.Setup(m => m.Create(It.IsAny<User>()))
               .Returns<User>(user =>
                {
                    _users.Add(MakeCopy(user));
                    return Task.CompletedTask;
                });
            UserRepositoryMock.Setup(m => m.Update(It.IsAny<User>()))
               .Returns<User>(user =>
                {
                    int index = _users.FindIndex(item => item.Id == user.Id);
                    _users[index] = MakeCopy(user);
                    return Task.FromResult(true);
                });
            UserRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()))
               .Returns<string>(id =>
                {
                    var postIds = _posts.Where(x => x.AuthorId == id).Select(x => x.Id).ToArray();
                    var commentIds = _comments.Where(x => postIds.Contains(x.PostId)).Select(x => x.Id).ToArray();
                    var responsesIds = _responses.Where(x => commentIds.Contains(x.CommentId)).Select(x => x.Id).ToArray();
                    _responseLikes.RemoveAll(x => responsesIds.Contains(x.EntityId) || x.UserId == id);
                    _commentLikes.RemoveAll(x => commentIds.Contains(x.EntityId) || x.UserId == id);
                    _postLikes.RemoveAll(x => postIds.Contains(x.EntityId) || x.UserId == id);
                    _responses.RemoveAll(x => responsesIds.Contains(x.Id));
                    _comments.RemoveAll(x => commentIds.Contains(x.Id));
                    _posts.RemoveAll(x => postIds.Contains(x.Id));
                    _hashes.RemoveAll(x => x.Id == id);
                    _users.RemoveAll(x => x.Id == id);
                    return Task.CompletedTask;
                });
        }

        private void SetupHashRepositoryMock()
        {
            HashRepositoryMock = new Mock<IHashRepository>();
            HashRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
               .Returns<string>(userId => Task.FromResult(MakeCopy(_hashes.SingleOrDefault(u => u.Id == userId))));
            HashRepositoryMock.Setup(m => m.CreateOrUpdate(It.IsAny<Hash>()))
               .Returns<Hash>(hash =>
                {
                    int index = _hashes.FindIndex(item => item.Id == hash.Id);

                    if (index == -1)
                    {
                        _hashes.Add(MakeCopy(hash));
                        return Task.CompletedTask;
                    }

                    _hashes[index] = MakeCopy(hash);
                    return Task.CompletedTask;
                });
            HashRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()))
               .Returns<string>(id =>
                {
                    _users.RemoveAll(x => x.Id == id);
                    return Task.CompletedTask;
                });
        }

        private void SetupPostRepositoryMock()
        {
            PostRepositoryMock = new Mock<IPostRepository>();
            PostRepositoryMock.Setup(m => m.GetPage(It.IsAny<int>(), It.IsAny<int>()))
               .Returns<int, int>((skip, take) => Task.FromResult(_posts.OrderByDescending(x => x.CreationTime)
                   .Skip(skip)
                   .Take(take).Select(MakeCopy).ToArray()));
            PostRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Post, bool>>>()))
               .Returns<Expression<Func<Post, bool>>>(predicate =>
                    Task.FromResult(_posts.Where(predicate.Compile().Invoke).Select(MakeCopy).ToArray()));
            PostRepositoryMock.Setup(m => m.GetById(It.IsAny<string>()))
               .Returns<string>(id => Task.FromResult(MakeCopy(_posts.SingleOrDefault(p => p.Id == id))));
            PostRepositoryMock.Setup(m => m.GetAll())
               .Returns(() => Task.FromResult(_posts.Select(MakeCopy).ToArray()));
            PostRepositoryMock.Setup(m => m.Create(It.IsAny<Post>()))
               .Returns<Post>(post =>
                {
                    _posts.Add(MakeCopy(post));
                    return Task.CompletedTask;
                });
            PostRepositoryMock.Setup(m => m.Update(It.IsAny<Post>()))
               .Returns<Post>(post =>
                {
                    int index = _posts.FindIndex(item => item.Id == post.Id);
                    _posts[index] = MakeCopy(post);
                    return Task.FromResult(true);
                });
            PostRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()))
               .Returns<string>(id =>
                {
                    var commentIds = _comments.Where(x => x.PostId == id).Select(x => x.Id).ToArray();
                    var responsesIds = _responses.Where(x => commentIds.Contains(x.CommentId)).Select(x => x.Id).ToArray();
                    _responseLikes.RemoveAll(x => responsesIds.Contains(x.EntityId));
                    _commentLikes.RemoveAll(x => commentIds.Contains(x.EntityId));
                    _postLikes.RemoveAll(x => x.EntityId == id);
                    _responses.RemoveAll(x => responsesIds.Contains(x.Id));
                    _comments.RemoveAll(x => x.PostId == id);
                    _posts.RemoveAll(x => x.Id == id);
                    return Task.CompletedTask;
                });
        }

        private void SetupCommentRepositoryMock()
        {
            CommentRepositoryMock = new Mock<ICommentRepository>();
            CommentRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Comment, bool>>>()))
               .Returns<Expression<Func<Comment, bool>>>(predicate =>
                    Task.FromResult(_comments.Where(predicate.Compile().Invoke).Select(MakeCopy).ToArray()));
            CommentRepositoryMock.Setup(m => m.GetById(It.IsAny<string>()))
               .Returns<string>(id => Task.FromResult(MakeCopy(_comments.SingleOrDefault(c => c.Id == id))));
            CommentRepositoryMock.Setup(m => m.GetAll())
               .Returns(() => Task.FromResult(_comments.Select(MakeCopy).ToArray()));
            CommentRepositoryMock.Setup(m => m.Create(It.IsAny<Comment>()))
               .Returns<Comment>(comment =>
               {
                   _comments.Add(MakeCopy(comment));
                   return Task.CompletedTask;
               });
            CommentRepositoryMock.Setup(m => m.Update(It.IsAny<Comment>()))
               .Returns<Comment>(comment =>
               {
                   int index = _comments.FindIndex(item => item.Id == comment.Id);
                   _comments[index] = MakeCopy(comment);
                   return Task.FromResult(true);
               });
            CommentRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()))
               .Returns<string>(id =>
               {
                   var responsesIds = _responses.Where(x => x.CommentId == id).Select(x => x.Id).ToArray();
                   _responses.RemoveAll(x => responsesIds.Contains(x.CommentId));
                   _responseLikes.RemoveAll(x => responsesIds.Contains(x.EntityId));
                   _comments.RemoveAll(x => x.Id == id);
                   _commentLikes.RemoveAll(x => x.EntityId == id);
                   return Task.CompletedTask;
               });
        }

        private void SetupResponseRepositoryMock()
        {
            ResponseRepositoryMock = new Mock<IResponseRepository>();
            ResponseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Response, bool>>>()))
               .Returns<Expression<Func<Response, bool>>>(predicate =>
                    Task.FromResult(_responses.Where(predicate.Compile().Invoke).Select(MakeCopy).ToArray()));
            ResponseRepositoryMock.Setup(m => m.GetById(It.IsAny<string>()))
               .Returns<string>(id => Task.FromResult(MakeCopy(_responses.SingleOrDefault(r => r.Id == id))));
            ResponseRepositoryMock.Setup(m => m.GetAll())
               .Returns(() => Task.FromResult(_responses.Select(MakeCopy).ToArray()));
            ResponseRepositoryMock.Setup(m => m.Create(It.IsAny<Response>()))
               .Returns<Response>(response =>
                {
                    _responses.Add(MakeCopy(response));
                    return Task.CompletedTask;
                });
            ResponseRepositoryMock.Setup(m => m.Update(It.IsAny<Response>()))
               .Returns<Response>(response =>
                {
                    int index = _comments.FindIndex(item => item.Id == response.Id);
                    _responses[index] = MakeCopy(response);
                    return Task.FromResult(true);
                });
            ResponseRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()))
               .Returns<string>(id =>
                {
                    _responses.RemoveAll(x => x.Id == id);
                    _responseLikes.RemoveAll(x => x.EntityId == id);
                    return Task.CompletedTask;
                });
        }

        private void SetupTagRepositoryMock()
        {
            TagRepositoryMock = new Mock<ITagRepository>();
            TagRepositoryMock.Setup(m => m.GetByName(It.IsAny<string>()))
               .Returns<string>(name => Task.FromResult(MakeCopy(_tags.SingleOrDefault(u => u.Name == name))));
            TagRepositoryMock.Setup(m => m.GetAll())
               .Returns(() => Task.FromResult(_tags.Select(MakeCopy).ToArray()));
            TagRepositoryMock.Setup(m => m.Create(It.IsAny<Tag>()))
               .Returns<Tag>(tag =>
                {
                    if(!_tags.Any(x => x.Name == tag.Name))
                        _tags.Add(MakeCopy(tag));
                    return Task.CompletedTask;
                });
            TagRepositoryMock.Setup(m => m.Create(It.IsAny<Tag[]>()))
               .Returns<Tag[]>(tags =>
                {
                    foreach (Tag tag in tags)
                    {
                        if (!_tags.Any(x => x.Name == tag.Name))
                            _tags.Add(MakeCopy(tag));
                    }
                    return Task.CompletedTask;
                });
            TagRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()))
               .Returns<string>(name =>
                {
                    _tags.RemoveAll(x => x.Name == name);
                    return Task.CompletedTask;
                });
        }

        private void SetupPostLikeRepositoryMock()
        {
            PostLikeRepositoryMock = new Mock<ILikeRepository<Post>>();
            PostLikeRepositoryMock.Setup(m => m.GetNumberOfLikes(It.IsAny<string>()))
               .Returns<string>(entityId =>
                    Task.FromResult(_postLikes.Count(x => x.EntityId == entityId && x.IsLike)));
            PostLikeRepositoryMock.Setup(m => m.GetNumberOfDislikes(It.IsAny<string>()))
               .Returns<string>(entityId =>
                    Task.FromResult(_postLikes.Count(x => x.EntityId == entityId && !x.IsLike)));
            PostLikeRepositoryMock.Setup(m => m.CancelLikeOrDislike(It.IsAny<string>(), It.IsAny<string>()))
               .Returns<string, string>((userId, entityId) =>
                {
                    _postLikes.RemoveAll(x => x.EntityId == entityId && x.UserId == userId);
                    return Task.CompletedTask;
                });
            PostLikeRepositoryMock.Setup(m => m.GiveLike(It.IsAny<User>(), It.IsAny<Post>(),It.IsAny<bool>()))
               .Returns<User,Post,bool>((user, post, isLike) =>
                {
                    int index = _postLikes.FindIndex(x => x.UserId == user.Id && x.EntityId == post.Id);

                    if(index == -1)
                    {
                        _postLikes.Add(new BaseLikeEntity<Context.Entities.Post>
                        {
                            EntityId = post.Id,
                            UserId = user.Id,
                            IsLike = isLike
                        });
                    }
                    else
                    {
                        _postLikes[index].IsLike = isLike;
                    }
                    return Task.CompletedTask;
                });
        }

        private void SetupCommentLikeRepositoryMock()
        {
            CommentLikeRepositoryMock = new Mock<ILikeRepository<Comment>>();
            CommentLikeRepositoryMock.Setup(m => m.GetNumberOfLikes(It.IsAny<string>()))
               .Returns<string>(entityId =>
                    Task.FromResult(_commentLikes.Count(x => x.EntityId == entityId && x.IsLike)));
            CommentLikeRepositoryMock.Setup(m => m.GetNumberOfDislikes(It.IsAny<string>()))
               .Returns<string>(entityId =>
                    Task.FromResult(_commentLikes.Count(x => x.EntityId == entityId && !x.IsLike)));
            CommentLikeRepositoryMock.Setup(m => m.CancelLikeOrDislike(It.IsAny<string>(), It.IsAny<string>()))
               .Returns<string, string>((userId, entityId) =>
                {
                    _commentLikes.RemoveAll(x => x.EntityId == entityId && x.UserId == userId);
                    return Task.CompletedTask;
                });
            CommentLikeRepositoryMock.Setup(m => m.GiveLike(It.IsAny<User>(), It.IsAny<Comment>(), It.IsAny<bool>()))
               .Returns<User, Comment, bool>((user, comment, isLike) =>
                {
                    int index = _commentLikes.FindIndex(x => x.UserId == user.Id && x.EntityId == comment.Id);

                    if (index == -1)
                    {
                        _commentLikes.Add(new BaseLikeEntity<Context.Entities.Comment>
                        {
                            EntityId = comment.Id,
                            UserId = user.Id,
                            IsLike = isLike
                        });
                    }
                    else
                    {
                        _commentLikes[index].IsLike = isLike;
                    }
                    return Task.CompletedTask;
                });
        }

        private void SetupResponseLikeRepositoryMock()
        {
            ResponseLikeRepositoryMock = new Mock<ILikeRepository<Response>>();
            ResponseLikeRepositoryMock.Setup(m => m.GetNumberOfLikes(It.IsAny<string>()))
               .Returns<string>(entityId =>
                    Task.FromResult(_responseLikes.Count(x => x.EntityId == entityId && x.IsLike)));
            ResponseLikeRepositoryMock.Setup(m => m.GetNumberOfDislikes(It.IsAny<string>()))
               .Returns<string>(entityId =>
                    Task.FromResult(_responseLikes.Count(x => x.EntityId == entityId && !x.IsLike)));
            ResponseLikeRepositoryMock.Setup(m => m.CancelLikeOrDislike(It.IsAny<string>(), It.IsAny<string>()))
               .Returns<string, string>((userId, entityId) =>
                {
                    _responseLikes.RemoveAll(x => x.EntityId == entityId && x.UserId == userId);
                    return Task.CompletedTask;
                });
            ResponseLikeRepositoryMock.Setup(m => m.GiveLike(It.IsAny<User>(), It.IsAny<Response>(), It.IsAny<bool>()))
               .Returns<User, Response, bool>((user, response, isLike) =>
                {
                    int index = _responseLikes.FindIndex(x => x.UserId == user.Id && x.EntityId == response.Id);

                    if (index == -1)
                    {
                        _responseLikes.Add(new BaseLikeEntity<Context.Entities.Response>
                        {
                            EntityId = response.Id,
                            UserId = user.Id,
                            IsLike = isLike
                        });
                    }
                    else
                    {
                        _responseLikes[index].IsLike = isLike;
                    }
                    return Task.CompletedTask;
                });
        }

        public Mock<IUserRepository> UserRepositoryMock { get; private set; }
        public Mock<IHashRepository> HashRepositoryMock { get; private set; }
        public Mock<IPictureRepository> PictureRepositoryMock { get; private set; }
        public Mock<IPostRepository> PostRepositoryMock { get; private set; }
        public Mock<ICommentRepository> CommentRepositoryMock { get; private set; }
        public Mock<IResponseRepository> ResponseRepositoryMock { get; private set; }
        public Mock<ITagRepository> TagRepositoryMock { get; private set; }
        public Mock<ILikeRepository<Post>> PostLikeRepositoryMock { get; private set; }
        public Mock<ILikeRepository<Comment>> CommentLikeRepositoryMock { get; private set; }
        public Mock<ILikeRepository<Response>> ResponseLikeRepositoryMock { get; private set; }

        private T MakeCopy<T>(T t) where T : BaseModel
        {
            string serializedObject = JsonSerializer.Serialize(t);
            return JsonSerializer.Deserialize<T>(serializedObject);
        }
        private Tag MakeCopy(Tag tag) =>
            new Tag
            {
                Name = tag.Name,
                PostsNumber = tag.PostsNumber
            };
    }
}