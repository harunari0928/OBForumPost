using OBFormPost.Application.ViewModel.Posts;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service
{
    public interface IPostControllerService
    {
        Task<IPostViewModel> Get(long postId);
        Task<PostViewModel> Create(CreateRequestModel request);
        Task<IPostViewModel> Update(PostViewModel originalPost);
        Task Remove(long postId);
    }
}
