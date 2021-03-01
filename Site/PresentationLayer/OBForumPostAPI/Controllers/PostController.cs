using Microsoft.AspNetCore.Mvc;
using OBFormPost.Application.Service;
using OBFormPost.Application.Service.Auth;
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
        private readonly IAuthService authService;

        public PostController(IPostControllerService postControllerService, IAuthService authService)
        {
            this.postControllerService = postControllerService;
            this.authService = authService;
        }

        [Route("{postId}")]
        [HttpGet]
        public async Task<ActionResult<IPostViewModel>> Get(int postId)
        {
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

        [HttpPost]
        public async Task<ActionResult<PostViewModel>> Create([FromHeader] string token, [FromBody] CreateRequestModel request)
        {
            if (!await authService.IsAuthenticated(token, Operation.CreatePost))
            {
                return Forbid();
            }

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

        [HttpPut]
        public async Task<ActionResult<PostViewModel>> Update([FromHeader] string token, [FromBody] PostViewModel originalPost)
        {
            if (!await authService.IsAuthenticated(token, Operation.UpdatePost))
            {
                return Forbid();
            }

            try
            {
                var updatedPost = await postControllerService.Update(originalPost);
                return Ok(updatedPost);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{postId}")]
        [HttpDelete]
        public async Task<ActionResult<PostViewModel>> Remove([FromHeader] string token, long postId)
        {
            if (!await authService.IsAuthenticated(token, Operation.RemovePost))
            {
                return Forbid();
            }

            try
            {
                await postControllerService.Remove(postId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
