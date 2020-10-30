using System;
using System.Text.Json;
using System.Threading.Tasks;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Posts;
using ContentAggregator.Services.Session;
using ContentAggregator.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ContentAggregator.UnitTests.Posts
{
    [TestFixture]
    public class PostServiceTests
    {
        #region Private

        private static async Task<PostService> GetService()
        {
            var hub = new MockRepositoriesHub();
            Mock<ISessionService> sessionService = await GetSessionServiceMockWhenFirstUserIsLogged(hub);
            var postService = new PostService(sessionService.Object,
                hub.PostRepositoryMock.Object,
                hub.TagRepositoryMock.Object,
                hub.PostLikeRepositoryMock.Object,
                new Mock<ILogger<PostService>>().Object);

            return postService;
        }

        private static async Task<Mock<ISessionService>> GetSessionServiceMockWhenFirstUserIsLogged(
            MockRepositoriesHub hub)
        {
            User user = await hub.UserRepositoryMock.Object.GetByUserName("kotwica407");
            Mock<ISessionService> sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(m => m.GetUser()).Returns(() => Task.FromResult(user));
            return sessionServiceMock;
        }

        private static async Task<Mock<ISessionService>> GetSessionServiceMockWhenNooneIsLogged()
        {
            Mock<ISessionService> sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(m => m.GetUser()).Returns(() => Task.FromResult((User)null));
            return sessionServiceMock;
        }

        #endregion

        [Test]
        public async Task GetPost()
        {
            PostService service = await GetService();
            Post actual = await service.Get("post-1");
            var expected = new Post
            {
                Id = "post-1",
                AuthorId = "user-1",
                Title = "Title of post no.1",
                Content = "Content of post no.1 #tag1 #tag2 \n" +
                    "#tag3",
                CreationTime = new DateTime(2020, 10, 1, 12, 0, 0),
                LastUpdateTime = new DateTime(2020, 10, 1, 12, 0, 0),
                Tags = new[] {"tag1", "tag2", "tag3"}
            };

            Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        [Test]
        public async Task CreatePostAndCheckIfCreated()
        {
            PostService service = await GetService();
            var post = await service.Create(new CreatePostDto
            {
                Title = "Title of post no.2",
                Content = "Content of post no.2 #tag4 #tag5 \n #tag6"
            });

            var expected = new Post
            {
                Title = "Title of post no.2",
                Content = "Content of post no.2 #tag4 #tag5 \n #tag6",
                Tags = new []{"tag4","tag5","tag6"},
                AuthorId = "user-1"
            };

            Assert.AreEqual(expected.Title, post.Title);
            Assert.AreEqual(expected.Content, post.Content);
            Assert.AreEqual(expected.AuthorId, post.AuthorId);
            Assert.That(expected.Tags, Is.EquivalentTo(post.Tags));
        }
    }
}