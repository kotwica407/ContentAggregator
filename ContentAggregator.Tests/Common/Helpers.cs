using System.Threading.Tasks;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Posts;
using ContentAggregator.Services.Session;
using ContentAggregator.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContentAggregator.UnitTests.Common
{
    internal static class Helpers
    {
        internal static async Task<PostService> GetService(bool userIsLogged)
        {
            var hub = new MockRepositoriesHub();
            Mock<ISessionService> sessionService = userIsLogged
                ? await GetSessionServiceMockWhenFirstUserIsLogged(hub)
                : GetSessionServiceMockWhenNooneIsLogged();
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

        private static Mock<ISessionService> GetSessionServiceMockWhenNooneIsLogged()
        {
            Mock<ISessionService> sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(m => m.GetUser()).Returns(() => Task.FromResult((User)null));
            return sessionServiceMock;
        }
    }
}