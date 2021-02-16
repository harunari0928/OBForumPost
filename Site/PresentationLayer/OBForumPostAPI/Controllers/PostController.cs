using Microsoft.AspNetCore.Mvc;
using OBFormPost.Application.RequestModel;
using OBFormPost.Application.Service;
using OBFormPost.Application.ViewModel.Posts;
using System;
using System.Threading.Tasks;

namespace OBForumPostAPI.Controllers
{
    [ApiController]
    [Route("post")]
    public class PostController : Controller
    {
        private readonly IPostControllerService postControllerService;
        public PostController(IPostControllerService postControllerService)
        {
            this.postControllerService = postControllerService;
        }

        [Route("{postId}")]
        [HttpGet]
        public async Task<ActionResult<IPostViewModel>> Index(int postId)
        {
            // TODO: 認証バリデーション
            try
            {
                var post = await postControllerService.Get(postId);
                return Ok(post);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult<PostViewModel>> Create([FromBody] CreateRequestModel request)
        {
            // TODO: 認証バリデーション
            try
            {
                var post = await postControllerService.Create(request);
                return Ok(post);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
