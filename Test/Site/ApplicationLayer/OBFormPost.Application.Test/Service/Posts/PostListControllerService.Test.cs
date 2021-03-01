using Moq;
using OBFormPost.Application.Service.Post;
using OBFormPost.Application.ViewModel.Posts;
using OBForumPost.Domain.Posts;
using OBForumPost.Domain.Repository;
using OBForumPost.Domain.Shared;
using System;
using System.Threading.Tasks;
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
                repositoryMock
                   .Setup(x => x.Create(It.IsAny<Post>()))
                   .ReturnsAsync(new Post());
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
            [Fact]
            public async Task 作られた投稿が返されること()
            {
                var request = new CreateRequestModel
                {
                    Status = 0,
                    Title = "ccc",
                    AuthorId = 43
                };
                var createdPost = new Post
                {
                    Id = 100,
                    PostStatus = (PostStatus)request.Status,
                    PostedDateTime = DateTimeOffset.Now,
                    UpdatedDateTime = DateTimeOffset.Now,
                    Title = request.Title,
                    Author = new Author
                    {
                        Id = request.AuthorId,
                        Name = "作者"
                    }
                };
                var repositoryMock = new Mock<IPostRepository>();
                repositoryMock
                    .Setup(x => x.Create(It.IsAny<Post>()))
                    .ReturnsAsync(createdPost);
                var service = new PostControllerService(repositoryMock.Object);

                var viewModel = await service.Create(request);

                Assert.Equal(createdPost.Id, viewModel.Id);
                Assert.Equal(createdPost.PostedDateTime.DateTime, viewModel.PostedDateTime);
                Assert.Equal(createdPost.UpdatedDateTime.DateTime, viewModel.UpdatedDateTime);
                Assert.Equal(request.Status, (int)viewModel.Status);
                Assert.Equal(request.Title, viewModel.Title);
            }
        }
        public sealed class UpdateTest
        {
            [Fact]
            public async Task 更新に成功した場合引数から渡された投稿と同様のものが返されること()
            {
                var originalPost = new PostViewModel
                {
                    Id = 10,
                    PostedDateTime = DateTime.Parse("2021/01/15 10:01"),
                    UpdatedDateTime = DateTime.Parse("2021/02/15 14:11"),
                    Status = PostStatus.Open,
                    Title = "更新される投稿",
                };
                var repositoryMock = new Mock<IPostRepository>();
                repositoryMock
                    .Setup(x => x.Get(It.Is<long>(y => y == originalPost.Id)))
                    .ReturnsAsync(new Post
                    {
                        Id = originalPost.Id,
                        PostedDateTime = originalPost.PostedDateTime,
                        PostStatus = originalPost.Status,
                        Title = originalPost.Title,
                        UpdatedDateTime = DateTimeOffset.Now,
                        Author = new Author { Id = 1, Name = "" }
                    });
                var service = new PostControllerService(repositoryMock.Object);

                var mayBeUpdatedPost = await service.Update(originalPost);

                if (mayBeUpdatedPost is PostViewModel updatedPost)
                {
                    repositoryMock
                        .Verify(x => x.Update(It.Is<Post>(x
                            => x.Id == originalPost.Id
                            && x.Title == originalPost.Title
                            && x.PostedDateTime.DateTime == originalPost.PostedDateTime)),
                            Times.Once);
                    Assert.Equal(originalPost.Id, updatedPost.Id);
                    Assert.Equal(originalPost.PostedDateTime, updatedPost.PostedDateTime);
                    Assert.NotEqual(originalPost.UpdatedDateTime, updatedPost.UpdatedDateTime); // 更新日時は現時刻
                    Assert.Equal(originalPost.Status, updatedPost.Status);
                    Assert.Equal(originalPost.Title, updatedPost.Title);
                }
                else
                {
                    throw new Exception();
                }
            }
            [Fact]
            public async Task 引数が不正の場合例外を吐くこと()
            {
                var originalPost = new PostViewModel
                {
                    Id = -1,
                    PostedDateTime = DateTime.Parse("2021/01/15 10:01"),
                    UpdatedDateTime = DateTime.Parse("2021/02/15 14:11"),
                    Status = PostStatus.Open,
                    Title = "更新される投稿",
                };
                var repositoryMock = new Mock<IPostRepository>();
                repositoryMock
                    .Setup(x => x.Get(It.Is<long>(y => y < 0)))
                    .ThrowsAsync(new ArgumentException());
                var service = new PostControllerService(repositoryMock.Object);

                await Assert.ThrowsAsync<ArgumentException>(() => service.Update(originalPost));
            }
        }
        public sealed class RemoveTest
        {
            [Fact]
            public async Task 引数が不正の場合例外を吐くこと()
            {
                var repositoryMock = new Mock<IPostRepository>();
                repositoryMock
                    .Setup(x => x.Remove(It.Is<long>(x => x < 0)))
                    .ThrowsAsync(new ArgumentException());
                var service = new PostControllerService(repositoryMock.Object);

                await Assert.ThrowsAsync<ArgumentException>(() => service.Remove(-1));
            }
        }
    }
}
