using Microsoft.AspNetCore.Mvc;
using OBFormPost.Application.Service;
using OBFormPost.Application.ViewModel;
using System;
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

        [Route("{page}/{pageSize}")]
        [HttpGet]
        public async Task<ActionResult<PostListViewModel>> Get(int page, int pageSize)
        {
            try
            {
                var list = await postListControllerService.Get(page, pageSize);
                return Ok(list);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
