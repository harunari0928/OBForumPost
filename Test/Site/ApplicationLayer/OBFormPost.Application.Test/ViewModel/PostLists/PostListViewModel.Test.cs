using OBFormPost.Application.ViewModel;
using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Shared;
using System;
using System.Linq;
using Xunit;

namespace OBFormPost.Application.Test.ViewModel
{
    public sealed class PostListViewModelTest
    {
        public sealed class CreateFromPostListTest
        {
            [Fact]
            public void PostListからViewModelを生成できること()
            {
                var post1 = new Post
                {
                    Id = 100,
                    PostedDateTime = DateTimeOffset.Parse("2019/03/22 15:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/09/22 15:00 +9:00"),
                    Author = new Author(),
                    PostStatus = PostStatus.Open,
                    Title = "たいとる"
                };
                var post2 = new Post
                {
                    Id = 100,
                    PostedDateTime = DateTimeOffset.Parse("2019/03/22 15:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/09/22 15:00 +9:00"),
                    Author = new Author(),
                    PostStatus = PostStatus.Open,
                    Title = "たいとる"
                };

                var domainModel = new PostList(new Post[] { post1, post2 });
                var viewModel = PostListViewModel.CreateFromPostList(domainModel);

                foreach (var (original, converted) in domainModel
                    .OrderBy(x => x.Id)
                    .Zip(viewModel.OrderBy(x => x.Id), (original, converted) => (original, converted)))
                {
                    Assert.Equal(original.Id, converted.Id);
                    Assert.Equal(original.Title, converted.Title);
                }
            }
        }
    }
}
