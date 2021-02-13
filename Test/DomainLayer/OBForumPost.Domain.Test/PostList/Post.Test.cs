using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OBForumPost.Domain.Test.PostLists
{
    public sealed class PostTest
    {
        [Fact]
        public void 更新日時が異なってもIdが等しければ同じ投稿だとみなすこと()
        {
            var id = 4L;
            var post1 = new Post
            {
                Id = id,
                PostedDateTime = DateTimeOffset.Now,
                UpdatedDateTime = DateTimeOffset.MinValue,
                Author = new Author(),
                PostStatus = PostStatus.Close,
                Title = "あああ"
            };
            var post2 = new Post
            {
                Id = id,
                PostedDateTime = DateTimeOffset.Now,
                UpdatedDateTime = DateTimeOffset.Now,
                Author = new Author(),
                PostStatus = PostStatus.Close,
                Title = "あああ"
            };
            Assert.Equal(post1, post2);
        }

        [Fact]
        public void ステータスが異なってもIdが等しければ同じ投稿だとみなすこと()
        {
            var id = 4L;
            var post1 = new Post
            {
                Id = id,
                PostedDateTime = DateTimeOffset.MaxValue,
                UpdatedDateTime = DateTimeOffset.MinValue,
                Author = new Author(),
                PostStatus = PostStatus.Close,
                Title = "あああ"
            };
            var post2 = new Post
            {
                Id = id,
                PostedDateTime = DateTimeOffset.MaxValue,
                UpdatedDateTime = DateTimeOffset.MinValue, 
                Author = new Author(),
                PostStatus = PostStatus.Open,
                Title = "あああ"
            };
            Assert.Equal(post1, post2);
        }

        [Fact]
        public void タイトルが異なってもIdが等しければ同じ投稿だとみなすこと()
        {
            var id = 4L;
            var post1 = new Post
            {
                Id = id,
                PostedDateTime = DateTimeOffset.MaxValue,
                UpdatedDateTime = DateTimeOffset.MinValue,
                Author = new Author(),
                PostStatus = PostStatus.Close,
                Title = "あああ"
            };
            var post2 = new Post
            {
                Id = id,
                PostedDateTime = DateTimeOffset.MaxValue,
                UpdatedDateTime = DateTimeOffset.MinValue,
                Author = new Author(),
                PostStatus = PostStatus.Close, 
                Title = "いいい"
            };
            Assert.Equal(post1, post2);
        }
    }
}
