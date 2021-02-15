using System;
using OBFormPost.Application.ViewModel.Posts;
using OBForumPost.Domain.Posts;
using OBForumPost.Domain.Shared;
using Xunit;

namespace OBFormPost.Application.Test.ViewModel
{
    public sealed class CreateFromPostTest
    {
        [Fact]
        public void 投稿からそのViewModelを生成できること()
        {
            var post = new Post
            {
                Id = 1,
                PostedDateTime = DateTimeOffset.Parse("2021/02/14 15:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2010/06/23 19:00 +9:00"),
                PostStatus = PostStatus.Close,
                Title = "テスト",
                Author = new Author
                {
                    Id = 2,
                    Name = "aaa"
                }
            };
            var acutal = PostViewModel.CreateFromPost(post);
            Assert.Equal(post.Id, acutal.Id);
            Assert.Equal(post.PostedDateTime.DateTime, acutal.PostedDateTime);
            Assert.Equal(post.UpdatedDateTime.DateTime, acutal.UpdatedDateTime);
            Assert.Equal(post.PostStatus, acutal.Status);
            Assert.Equal(post.Title, acutal.Title);
        }
    }
}
