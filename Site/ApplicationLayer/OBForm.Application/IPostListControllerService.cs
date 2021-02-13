using OBFormPost.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service
{
    public interface IPostListControllerService
    {
        Task<PostListViewModel> Get(int page, int pageSize);
    }
}
