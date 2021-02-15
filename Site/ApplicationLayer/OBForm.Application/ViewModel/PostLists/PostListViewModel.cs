using OBFormPost.Application.ViewModel.PostLists;
using OBForumPost.Domain.PostLists;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OBFormPost.Application.ViewModel
{
    public sealed class PostListViewModel : IEnumerable<PostLists.PostViewModel>
    {
        private readonly IEnumerable<PostLists.PostViewModel> posts;

        public PostListViewModel(IEnumerable<PostLists.PostViewModel> posts)
        {
            this.posts = posts;
        }

        public static PostListViewModel CreateFromPostList(PostList postList)
        {
            return new PostListViewModel(postList.Select(PostLists.PostViewModel.CreateFromPost));
        }

        public IEnumerator<PostLists.PostViewModel> GetEnumerator()
        {
            foreach (var post in posts)
            {
                yield return post;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
