using System.Linq;
using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OBForumPost.Domain.Test.PostLists
{
    public sealed class PostListTest
    {
        [Fact]
        public void IEnumeraleのインタフェース実装が正しく行われていること()
        {
            var originalPosts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Author = new Author(),
                    PostedDateTime = DateTimeOffset.Now,
                    PostStatus = PostStatus.Close,
                    Title = "テストタイトル"
                },
                new Post
                {
                    Id = 1,
                    Author = new Author(),
                    PostedDateTime = DateTimeOffset.Now,
                    PostStatus = PostStatus.Open,
                    Title = "テストタイトル2"
                },
                new Post
                {
                    Id = 1,
                    Author = new Author(),
                    PostedDateTime = DateTimeOffset.Now,
                    PostStatus = PostStatus.Close,
                    Title = "テストタイトル3"
                }
            };
            var postList = new PostList(originalPosts);
            Assert.True(originalPosts.SequenceEqual(postList));
        }
    }
}
