using OBForumPost.Domain.Shared;
using System;

namespace OBForumPost.Domain.PostLists
{
    /// <summary>
    /// 投稿リスト内の投稿
    /// </summary>
    public sealed class Post : IPost
    {
        public long Id { get; init; }
        public DateTimeOffset PostedDateTime { get; init; }
        public DateTimeOffset UpdatedDateTime { get; init; }
        public PostStatus PostStatus { get; init; }
        public string Title { get; init; }
        public Author Author { get; init; }

        public override bool Equals(object? obj) => obj switch
        {
            Post target => Id == target.Id,
            _ => false,
        };


        public override int GetHashCode() => Id.GetHashCode();
    }
}
