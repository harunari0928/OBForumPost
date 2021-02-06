using OBFormPost.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service
{
    public interface IPostControllerService
    {
        public Task<PostFountViewModel> Get(long postId);
    }
}
