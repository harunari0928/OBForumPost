using System;

namespace OBForumPost.Domain.Shared
{
    public interface IPost
    {
        long Id { get; }
        DateTimeOffset PostedDateTime { get; }
        PostStatus PostStatus { get; }
        string Title { get; }
        Author Author { get; }
    }
}
