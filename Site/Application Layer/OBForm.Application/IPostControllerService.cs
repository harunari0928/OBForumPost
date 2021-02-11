using OBFormPost.Application.ViewModel;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service
{
    public interface IPostControllerService
    {
        public Task<PostFountViewModel> Get(long postId);
    }
}
