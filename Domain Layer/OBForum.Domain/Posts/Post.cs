using OBForumPost.Domain.Shared;
using System;

namespace OBForumPost.Domain.Posts
{
    /// <summary>
    /// 投稿
    /// </summary>
    public sealed class Post : IPost, IAggregateRoot
    {
        public long Id { get; private set; }
        public DateTimeOffset PostedDateTime { get; private set; }
        public PostStatus PostStatus { get; private set; }
        public string Title { get; private set; }
        public Author Author { get; private set; }
    }
}
