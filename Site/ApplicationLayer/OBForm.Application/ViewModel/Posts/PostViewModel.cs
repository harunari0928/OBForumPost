using System;
using OBForumPost.Domain.Posts;
using OBForumPost.Domain.Shared;

namespace OBFormPost.Application.ViewModel.Posts
{
    public sealed class PostViewModel : IPostViewModel
    {
        public long Id { get; init; }
        public PostStatus Status { get; init; }
        public DateTime PostedDateTime { get; init; }
        public DateTime UpdatedDateTime { get; init; }
        public string Title { get; init; }

        public static PostViewModel CreateFromPost(Post post)
        {
            return new PostViewModel
            {
                Id = post.Id,
                PostedDateTime = post.PostedDateTime.DateTime,
                UpdatedDateTime = post.UpdatedDateTime.DateTime,
                Status = post.PostStatus,
                Title = post.Title
            };
        }
    }
}
