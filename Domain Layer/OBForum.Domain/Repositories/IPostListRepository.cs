using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OBForumPost.Domain.PostLists;

namespace OBForumPost.Domain.Repository
{
    public interface IPostListRepository
    {
        Task<PostList> Get(int page, int pageSize);
    }
}
