using OBFormPost.Application.RequestModel;
using OBFormPost.Application.ViewModel.Posts;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service
{
    public interface IPostControllerService
    {
        Task<IPostViewModel> Get(long postId);
        Task Create(CreateRequestModel request);
    }
}
