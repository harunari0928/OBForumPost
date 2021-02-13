using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OBForumPost.Domain.PostLists
{
    /// <summary>
    /// 投稿リスト
    /// </summary>
    public sealed class PostList : IAggregateRoot, IReadOnlyList<Post>
    {
        private readonly IReadOnlyList<Post> posts;

        public PostList(IEnumerable<Post> posts)
        {
            this.posts = posts.ToList();
        }

        public Post this[int index] => posts[index];

        public int Count => posts.Count;

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
