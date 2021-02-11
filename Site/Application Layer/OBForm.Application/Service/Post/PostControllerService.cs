using OBFormPost.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBFormPost.Application.Service.Post
{
    public sealed class PostControllerService : IPostControllerService
    {
        public Task<PostFountViewModel> Get(long postId)
        {
            throw new NotImplementedException();
        }
    }
}
