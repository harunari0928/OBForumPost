using OBFormPost.Application.ViewModel;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service
{
    public interface IPostListControllerService
    {
        Task<PostListViewModel> Get(int page, int pageSize);
    }
}
