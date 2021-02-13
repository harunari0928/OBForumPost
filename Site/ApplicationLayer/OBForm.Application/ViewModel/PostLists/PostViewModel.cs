using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Shared;
using System;

namespace OBFormPost.Application.ViewModel.PostLists
{
    public sealed class PostViewModel
    {
        public long Id { get; init; }
        public DateTime PostedDateTime { get; init; }
        public DateTime UpdateDateTime { get; init; }
        public string PostStatus { get; init; }
        public string Title { get; init; }

        public static PostViewModel CreateFromPost(Post post)
        {
            return new PostViewModel
            {
                Id = post.Id,
                PostedDateTime = post.PostedDateTime.ToLocalTime().DateTime,
                UpdateDateTime = post.UpdatedDateTime.ToLocalTime().DateTime,
                PostStatus = GetStatusStiring(post.PostStatus),
                Title = post.Title
            };
        }

        private static string GetStatusStiring(PostStatus status) => status switch
        {
            OBForumPost.Domain.Shared.PostStatus.Open => "オープン",
            OBForumPost.Domain.Shared.PostStatus.Close => "クローズ",
            _ => throw new NotSupportedException(),
        };
    }
}
