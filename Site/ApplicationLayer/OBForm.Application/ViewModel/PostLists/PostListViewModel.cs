using OBFormPost.Application.ViewModel.PostLists;
using OBForumPost.Domain.PostLists;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OBFormPost.Application.ViewModel
{
    public sealed class PostListViewModel : IEnumerable<PostViewModel>
    {
        private readonly IEnumerable<PostViewModel> posts;

        PostListViewModel(IEnumerable<PostViewModel> posts)
        {
            this.posts = posts;
        }

        public static PostListViewModel CreateFromPostList(PostList postList)
        {
            return new PostListViewModel(postList.Select(PostViewModel.CreateFromPost));
        }

        public IEnumerator<PostViewModel> GetEnumerator()
        {
            foreach (var post in posts)
            {
                yield return post;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
