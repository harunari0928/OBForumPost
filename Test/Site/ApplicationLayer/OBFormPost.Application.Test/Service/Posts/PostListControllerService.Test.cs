using System;
using System.Threading.Tasks;
using Moq;
using OBFormPost.Application.RequestModel;
using OBFormPost.Application.Service.Post;
using OBFormPost.Application.ViewModel.Posts;
using OBForumPost.Domain.Posts;
using OBForumPost.Domain.Repository;
using OBForumPost.Domain.Shared;
using Xunit;

namespace OBFormPost.Application.Test.Service.Posts
{
    public sealed class PostListControllerServiceTest
    {
        public sealed class GetTest
        {
            [Fact]
            public async Task 指定したIDの投稿ビューモデルを返すこと()
            {
                var post = new Post
                {
                    Id = 534,
                    PostStatus = PostStatus.Open,
                    PostedDateTime = DateTimeOffset.Parse("2021/02/15 09:11 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/02/16 09:11 +9:00"),
                    Title = "たいとる"
                };
                var repositoryMock = new Mock<IPostRepository>();
                repositoryMock
                    .Setup(x => x.Get(post.Id))
                    .ReturnsAsync(post);

                var service = new PostControllerService(repositoryMock.Object);
                var viewModel = await service.Get(post.Id);

                Assert.IsType<PostViewModel>(viewModel);
                if (viewModel is PostViewModel actual)
                {
                    Assert.Equal(post.Id, actual.Id);
                    Assert.Equal(post.PostStatus, actual.Status);
                    Assert.Equal(post.PostedDateTime.DateTime, actual.PostedDateTime);
                    Assert.Equal(post.UpdatedDateTime.DateTime, actual.UpdatedDateTime);
                    Assert.Equal(post.Title, actual.Title);
                }
            }

            [Fact]
            public async Task 指定したIDの投稿がないときその旨のオブジェクトを返すこと()
            {
                var repositoryMock = new Mock<IPostRepository>();
                var service = new PostControllerService(repositoryMock.Object);
                var viewModel = await service.Get(int.MaxValue);

                Assert.IsType<PostNotFoundViewModel>(viewModel);
            }
        }

        public sealed class CreateTest
        {
            [Fact]
            public async Task リクエスト通りに投稿を作成すること()
            {
                var repositoryMock = new Mock<IPostRepository>();
                var service = new PostControllerService(repositoryMock.Object);
                var request = new CreateRequestModel
                {
                    Status = 0,
                    Title = "ccc",
                    AuthorId = 43
                };
                await service.Create(request);

                repositoryMock
                    .Verify(x => x.Create(It.Is<Post>(y =>
                     y.Title == request.Title
                     && y.PostStatus == (PostStatus)request.Status
                     && y.Author.Id == request.AuthorId
                     )),
                      Times.Once);
            }
        }
    }
}
