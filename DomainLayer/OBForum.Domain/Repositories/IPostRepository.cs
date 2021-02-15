using OBForumPost.Domain.Posts;
using System.Threading.Tasks;

namespace OBForumPost.Domain.Repository
{
    public interface IPostRepository
    {
        Task<Post?> Get(long id);
        Task<Post> Create(Post post);
        Task Remove(long id);
        Task Update(Post post);
    }
}
