using Moq;
using OBFormPost.Application.Service.PostList;
using OBFormPost.Application.ViewModel;
using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Repository;
using OBForumPost.Domain.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OBFormPost.Application.Test.Service.PostLists
{
    public sealed class PostListControllerServiceTest
    {
        public sealed class GetTest
        {
            [Fact]
            public async Task ViewModelを返すこと()
            {
                var page = 3;
                var pageSize = 10;
                var postList = new PostList(new Post[]
                {
                    new Post
                    {
                        Id = 100,
                        Author = new Author(),
                        PostedDateTime = DateTimeOffset.Now,
                        UpdatedDateTime = DateTimeOffset.Now,
                        Title = "aaa"
                    },
                    new Post
                    {
                        Id = 120,
                        Author = new Author(),
                        PostedDateTime = DateTimeOffset.Now,
                        UpdatedDateTime = DateTimeOffset.Now,
                        Title = "bbbn"
                    },
                    new Post
                    {
                        Id = 999,
                        Author = new Author(),
                        PostedDateTime = DateTimeOffset.Now,
                        UpdatedDateTime = DateTimeOffset.Now,
                        Title = "ccc"
                    }
                });
                var repositoryMock = new Mock<IPostListRepository>();
                repositoryMock
                    .Setup(x => x.Get(page, pageSize, OrderByOptions.None))
                    .ReturnsAsync(postList);
                var applicationServce = new PostListControllerService(repositoryMock.Object);
                var expectedList = (await applicationServce.Get(page, pageSize)).OrderBy(x => x.Id);
                var actualList = PostListViewModel.CreateFromPostList(postList).OrderBy(x => x.Id);

                foreach (var (expected, actual) in expectedList.Zip(actualList, (e, a) => (e, a)))
                {
                    Assert.Equal(expected.Id, actual.Id);
                    Assert.Equal(expected.Title, actual.Title);
                }
            }
        }
    }
}
