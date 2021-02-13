using OBForumPost.Domain.PostLists;
using System.Threading.Tasks;

namespace OBForumPost.Domain.Repository
{
    public interface IPostListRepository
    {
        Task<PostList> Get(int page, int pageSize, OrderByOptions orderByOption = OrderByOptions.None);
    }

    public enum OrderByOptions
    {
        None = -1,
        PostedDateTimeAsc = 0,
        PostedDateTimeDsc = 1,
        UpdatedDateTimeAsc = 2,
        UpdatedDateTimeDsc = 3
    }
}
