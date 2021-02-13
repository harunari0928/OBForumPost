using OBFormPost.Application.ViewModel;
using OBForumPost.Domain.Repository;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service.PostList
{
    public sealed class PostListControllerService : IPostListControllerService
    {
        private readonly IPostListRepository repository;
        public PostListControllerService(IPostListRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PostListViewModel> Get(int page, int pageSize)
        {
            var postList = await repository.Get(page, pageSize);
            return PostListViewModel.CreateFromPostList(postList);
        }
    }
}
