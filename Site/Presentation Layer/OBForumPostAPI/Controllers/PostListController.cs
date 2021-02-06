using Microsoft.AspNetCore.Mvc;
using OBFormPost.Application.Service;
using OBFormPost.Application.ViewModel;
using System.Threading.Tasks;

namespace OBForumPostAPI.Controllers
{
    [ApiController]
    [Route("postList")]
    public class PostListController : Controller
    {
        private readonly IPostListControllerService postListControllerService;
        public PostListController(IPostListControllerService postListControllerService)
        {
            this.postListControllerService = postListControllerService;
        }

        [HttpGet]
        public async Task<ActionResult<PostListViewModel>> Index()
        {
            // TODO: 認証バリデーション

            var list = await postListControllerService.Get(0, 0);
            return Ok(list);
        }
    }
}
