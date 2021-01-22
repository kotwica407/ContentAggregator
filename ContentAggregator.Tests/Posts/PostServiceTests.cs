using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Posts;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Services.Posts;
using ContentAggregator.UnitTests.Common;
using NUnit.Framework;

namespace ContentAggregator.UnitTests.Posts
{
    [TestFixture]
    public class PostServiceTests
    {
        [Test]
        public async Task CreatePostAndCheckIfCreated()
        {
            PostService service = await Helpers.GetService(true);
            Post post = await service.Create(new CreatePostDto
            {
                Title = "Title of post no.2",
                Content = "Content of post no.2 #tag4 #tag5 \n #tag6"
            });

            var expected = new Post
            {
                Title = "Title of post no.2",
                Content = "Content of post no.2 #tag4 #tag5 \n #tag6",
                Tags = new[] {"tag4", "tag5", "tag6"},
                AuthorId = "user-1"
            };

            Assert.AreEqual(expected.Title, post.Title);
            Assert.AreEqual(expected.Content, post.Content);
            Assert.AreEqual(expected.AuthorId, post.AuthorId);
            Assert.That(expected.Tags, Is.EquivalentTo(post.Tags));
        }

        [Test]
        public async Task DeletePostAndCheckIfItIsDeleted()
        {
            PostService service = await Helpers.GetService(true);
            await service.Delete("post-1");

            AsyncTestDelegate testedDelegate = async () => await service.Get("post-1");

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.NotFound, httpResponseException.HttpStatusCode);
        }

        [Test]
        public async Task GetPost()
        {
            PostService service = await Helpers.GetService(true);
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
        [TestCase(true)]
        [TestCase(false)]
        public async Task LikePostAndCancelLikeAndCheckIfItIsNotLiked(bool isLike)
        {
            PostService service = await Helpers.GetService(true);
            await service.Rate("post-1", new RateDto {IsLike = isLike});
            await service.CancelRate("post-1");
            Post post = await service.Get("post-1");

            int actualLikes = post.Likes;
            int actualDislikes = post.Dislikes;

            Assert.AreEqual(0, actualLikes);
            Assert.AreEqual(0, actualDislikes);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task LikePostAndChangeLikeAndCheckIfItIsChanged(bool isLike)
        {
            PostService service = await Helpers.GetService(true);
            await service.Rate("post-1", new RateDto {IsLike = isLike});
            await service.Rate("post-1", new RateDto {IsLike = !isLike});
            Post post = await service.Get("post-1");

            int actualLikes = post.Likes;
            int actualDislikes = post.Dislikes;

            Assert.AreEqual(isLike ? 0 : 1, actualLikes);
            Assert.AreEqual(isLike ? 1 : 0, actualDislikes);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task LikePostAndCheckIfItIsLiked(bool isLike)
        {
            PostService service = await Helpers.GetService(true);
            await service.Rate("post-1", new RateDto {IsLike = isLike});
            Post post = await service.Get("post-1");
            int actualLikes = post.Likes;
            int actualDislikes = post.Dislikes;

            Assert.AreEqual(isLike ? 1 : 0, actualLikes);
            Assert.AreEqual(isLike ? 0 : 1, actualDislikes);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task LikePostTwiceAndCheckIfItIsLikedOnce(bool isLike)
        {
            PostService service = await Helpers.GetService(true);
            await service.Rate("post-1", new RateDto {IsLike = isLike});
            await service.Rate("post-1", new RateDto {IsLike = isLike});
            Post post = await service.Get("post-1");
            int actualLikes = post.Likes;
            int actualDislikes = post.Dislikes;

            Assert.AreEqual(isLike ? 1 : 0, actualLikes);
            Assert.AreEqual(isLike ? 0 : 1, actualDislikes);
        }

        [Test]
        public async Task TryToCreatePostWhenUnauthorizedAndCheckIfThrowsCorrectException()
        {
            PostService service = await Helpers.GetService(false);

            AsyncTestDelegate testedDelegate = async () => await service.Create(new CreatePostDto
            {
                Title = "Title of post no.2",
                Content = "Content of post no.2 #tag4 #tag5 \n #tag6"
            });

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.Unauthorized, httpResponseException.HttpStatusCode);
            Assert.AreEqual("You cannot create post if you are not logged in", httpResponseException.Message);
        }

        [Test]
        public async Task TryToCreatePostWithContentTooLongAndCheckIfThrowsCorrectException()
        {
            PostService service = await Helpers.GetService(true);

            AsyncTestDelegate testedDelegate = async () => await service.Create(new CreatePostDto
            {
                Title = "Title of post no.2",
                Content = new string('a', Consts.PostContentLength + 1)
            });

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseException.HttpStatusCode);
            Assert.AreEqual("Content is too long", httpResponseException.Message);
        }

        [Test]
        public async Task TryToCreatePostWithTitleTooLongAndCheckIfThrowsCorrectException()
        {
            PostService service = await Helpers.GetService(true);

            AsyncTestDelegate testedDelegate = async () => await service.Create(new CreatePostDto
            {
                Title = new string('a', Consts.PostTitleLength + 1),
                Content = "Content of post no.2 #tag4 #tag5 \n #tag6"
            });

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseException.HttpStatusCode);
            Assert.AreEqual("Title is too long", httpResponseException.Message);
        }

        [Test]
        public async Task TryToDeletePostOfAnotherUserAndCheckIfThrowsCorrectException()
        {
            PostService service = await Helpers.GetService(true);
            AsyncTestDelegate testedDelegate = async () => await service.Delete("post-2");

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.Forbidden, httpResponseException.HttpStatusCode);
            Assert.AreEqual("Post post-2 does not belong to user", httpResponseException.Message);
        }

        [Test]
        public async Task TryToDeletePostWhichDoesNotExistAndCheckIfThrowsCorrectException()
        {
            PostService service = await Helpers.GetService(true);
            AsyncTestDelegate testedDelegate = async () => await service.Delete("post-3");

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.NotFound, httpResponseException.HttpStatusCode);
            Assert.AreEqual("Post post-3 does not exist", httpResponseException.Message);
        }

        [Test]
        public async Task TryToUpdatePostOfAnotherUserAndCheckIfThrowsCorrectException()
        {
            PostService service = await Helpers.GetService(true);
            AsyncTestDelegate testedDelegate = async () => await service.Update("post-2",
                new UpdatePostDto
                {
                    Title = "Changed title of post no.2",
                    Content = "Changed content of post no.2 #tag5 \n #tag7 #tag8"
                });

            var httpResponseException = Assert.ThrowsAsync<HttpErrorException>(testedDelegate);
            Assert.AreEqual(HttpStatusCode.Forbidden, httpResponseException.HttpStatusCode);
            Assert.AreEqual("Post post-2 does not belong to user", httpResponseException.Message);
        }

        [Test]
        public async Task UpdatePostAndCheckIfUpdated()
        {
            PostService service = await Helpers.GetService(true);
            await service.Update("post-1",
                new UpdatePostDto
                {
                    Title = "Changed title of post no.1",
                    Content = "Changed content of post no.1 #tag5 \n #tag7 #tag8"
                });

            Post post = await service.Get("post-1");

            var expected = new Post
            {
                Title = "Changed title of post no.1",
                Content = "Changed content of post no.1 #tag5 \n #tag7 #tag8",
                Tags = new[] {"tag5", "tag7", "tag8"},
                AuthorId = "user-1"
            };

            Assert.AreEqual(expected.Title, post.Title);
            Assert.AreEqual(expected.Content, post.Content);
            Assert.AreEqual(expected.AuthorId, post.AuthorId);
            Assert.That(expected.Tags, Is.EquivalentTo(post.Tags));
        }
    }
}