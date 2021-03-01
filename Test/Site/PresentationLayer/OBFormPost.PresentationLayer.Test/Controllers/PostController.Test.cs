using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using OBFormPost.Application.Service;
using OBFormPost.Application.Service.Auth;
using OBFormPost.Application.ViewModel.Posts;
using OBForumPostAPI.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OBFormPost.PresentationLayer.Test.Controllers
{
    public sealed class PostControllerTest
    {
        public sealed class GetTest
        {
            [Fact]
            public async Task リクエストが不正なときBadRequestが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Get(-1))
                    .ThrowsAsync(new ArgumentException());
                var mockAuthService = new Mock<IAuthService>();
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                Assert.IsType<BadRequestObjectResult>((await controller.Get(-1)).Result);
            }

            [Fact]
            public async Task リクエストが正常なときOkとともに投稿が返ること()
            {
                var postId = 10;
                var post = new PostViewModel
                {
                    Id = postId,
                    Title = "tatitoru",
                    PostedDateTime = DateTime.Now,
                    UpdatedDateTime = DateTime.Now,
                    Status = OBForumPost.Domain.Shared.PostStatus.Close
                };
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Get(postId))
                    .ReturnsAsync(post);
                var mockAuthService = new Mock<IAuthService>();
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                var response = await controller.Get(postId);

                Assert.IsType<OkObjectResult>((await controller.Get(postId)).Result);
                var okObjectResult = (await controller.Get(postId)).Result as OkObjectResult;
                Assert.Equal(JsonConvert.SerializeObject(post), JsonConvert.SerializeObject(okObjectResult.Value));
            }
        }

        public sealed class CreateTest
        {
            [Fact]
            public async Task ユーザに権限がないときForbitが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.CreatePost))
                    .ReturnsAsync(false);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                var response = await controller.Create("invalid token", new CreateRequestModel { });

                Assert.IsType<ForbidResult>(response.Result);
            }

            [Fact]
            public async Task リクエストが正常なときOkとともに作った投稿が返されること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Create(It.IsAny<CreateRequestModel>()))
                    .ReturnsAsync(new PostViewModel { Id = 5 });
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.CreatePost))
                    .ReturnsAsync(true);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);
                var createRequest = new CreateRequestModel { Status = 0, Title = "fasdff", AuthorId = 1 };

                var response = await controller.Create("token", createRequest);

                Assert.IsType<OkObjectResult>(response.Result);
                var okObjectResult = response.Result as OkObjectResult;
                Assert.IsType<PostViewModel>(okObjectResult.Value);
            }

            [Fact]
            public async Task リクエストが不正なときBadRequestが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Create(It.IsAny<CreateRequestModel>()))
                    .ThrowsAsync(new ArgumentException());
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.CreatePost))
                    .ReturnsAsync(true);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                Assert.IsType<BadRequestObjectResult>((await controller.Create("token", new CreateRequestModel { })).Result);
            }
        }

        public sealed class UpdateTest
        {
            [Fact]
            public async Task ユーザに権限がないときForbitが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.UpdatePost))
                    .ReturnsAsync(false);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                var response = await controller.Update("invalid token", new PostViewModel { Id = 5, Title = "aaa" });

                Assert.IsType<ForbidResult>(response.Result);
            }

            [Fact]
            public async Task リクエストが正常なときOkとともに更新した投稿が返されること()
            {
                var updatedPost = new PostViewModel
                {
                    Id = 12,
                    PostedDateTime = DateTime.MinValue,
                    UpdatedDateTime = DateTime.Now,
                    Status = OBForumPost.Domain.Shared.PostStatus.Open,
                    Title = "saaa"
                };
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Update(It.IsAny<PostViewModel>()))
                    .ReturnsAsync(updatedPost);
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.UpdatePost))
                    .ReturnsAsync(true);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                var response = (await controller.Update("token", new PostViewModel { Id = updatedPost.Id }));
                Assert.IsType<OkObjectResult>(response.Result);
                var okObjectResult = response.Result as OkObjectResult;
                Assert.Equal(JsonConvert.SerializeObject(updatedPost), JsonConvert.SerializeObject(okObjectResult.Value));
            }

            [Fact]
            public async Task リクエストが不正なときBadRequestが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Update(It.IsAny<PostViewModel>()))
                    .ThrowsAsync(new ArgumentException());
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.UpdatePost))
                    .ReturnsAsync(true);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                Assert.IsType<BadRequestObjectResult>((await controller.Update("token", new PostViewModel { })).Result);
            }
        }

        public sealed class RemoveTest
        {
            [Fact]
            public async Task ユーザに権限がないときForbitが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.RemovePost))
                    .ReturnsAsync(false);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                var response = await controller.Remove("invalid token", 5);

                Assert.IsType<ForbidResult>(response.Result);
            }

            [Fact]
            public async Task リクエストが正常なときOkが返されること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.RemovePost))
                    .ReturnsAsync(true);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                Assert.IsType<OkResult>((await controller.Remove("token", 5)).Result);
            }

            [Fact]
            public async Task リクエストが不正なときBadRequestが返ること()
            {
                var mockControllerService = new Mock<IPostControllerService>();
                mockControllerService
                    .Setup(x => x.Remove(-1))
                    .ThrowsAsync(new ArgumentException());
                var mockAuthService = new Mock<IAuthService>();
                mockAuthService
                    .Setup(x => x.IsAuthenticated(It.IsAny<string>(), Operation.RemovePost))
                    .ReturnsAsync(true);
                var controller = new PostController(mockControllerService.Object, mockAuthService.Object);

                Assert.IsType<BadRequestObjectResult>((await controller.Remove("token", -1)).Result);
            }
        }
    }
}
