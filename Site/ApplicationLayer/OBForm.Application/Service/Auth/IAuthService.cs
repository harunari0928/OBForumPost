using System.Threading.Tasks;

namespace OBFormPost.Application.Service.Auth
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated(string token, Operation operation);
    }

    public enum Operation
    {
        CreatePost = 0,
        UpdatePost = 1,
        RemovePost = 2
    }

    public sealed class DummyAuthService : IAuthService
    {
        public Task<bool> IsAuthenticated(string _, Operation __)
        {
            return Task.Run(() => true);
        }
    }
}
