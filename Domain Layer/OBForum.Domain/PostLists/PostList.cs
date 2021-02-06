using System.Collections;
using System.Collections.Generic;

namespace OBForumPost.Domain.PostLists
{
    /// <summary>
    /// 投稿リスト
    /// </summary>
    public sealed class PostList : IAggregateRoot, IEnumerable<Post>
    {
        private readonly IEnumerable<Post> posts;

        public PostList(IEnumerable<Post> posts)
        {
            this.posts = posts;
        }

        public IEnumerator<Post> GetEnumerator()
        {
            foreach (var post in posts)
            {
                yield return post;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
