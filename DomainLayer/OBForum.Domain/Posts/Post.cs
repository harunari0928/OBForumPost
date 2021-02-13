using OBForumPost.Domain.Shared;
using System;

namespace OBForumPost.Domain.Posts
{
    /// <summary>
    /// 投稿
    /// </summary>
    public sealed class Post : IPost, IAggregateRoot
    {
        public long Id { get; init; }
        public DateTimeOffset PostedDateTime { get; init; }
        public DateTimeOffset UpdatedDateTime { get; init; }
        public PostStatus PostStatus { get; init; }
        public string Title { get; init; }
        public Author Author { get; init; }
    }
}
