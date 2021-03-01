using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using OBFormPost.Application.Service;
using OBFormPost.Application.ViewModel;
using OBFormPost.Application.ViewModel.PostLists;
using OBForumPostAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OBFormPost.PresentationLayer.Test.Controllers
{
    public sealed class PostListControllerTest
    {
        public sealed class GetTest
        {
            [Fact]
            public async Task 正常なリクエストの際は投稿リストと共にOkが返ること()
            {
                var postList = new PostListViewModel(new PostViewModel[]
                {
                    new PostViewModel
                    {
                        Id = 1,
                        Title = "111"
                    },
                    new PostViewModel
                    {
                        Id = 2,
                        Title = "222"
                    }
                });
                var mockPostListControllerService = new Mock<IPostListControllerService>();
                mockPostListControllerService
                    .Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(postList);

                var controller = new PostListController(mockPostListControllerService.Object);

                var response = await controller.Get(1, 1);
                Assert.IsType<OkObjectResult>(response.Result);
                var okObjectResult = response.Result as OkObjectResult;
                Assert.Equal(JsonConvert.SerializeObject(postList), JsonConvert.SerializeObject(okObjectResult.Value));
            }

            [Fact]
            public async Task 不正なリクエストの際はBadRequestが返ること()
            {
                var mockPostListControllerService = new Mock<IPostListControllerService>();
                mockPostListControllerService
                    .Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>()))
                    .ThrowsAsync(new ArgumentException());

                var controller = new PostListController(mockPostListControllerService.Object);

                Assert.IsType<BadRequestObjectResult>((await controller.Get(-1, 1)).Result);
            }
        }
    }
}
