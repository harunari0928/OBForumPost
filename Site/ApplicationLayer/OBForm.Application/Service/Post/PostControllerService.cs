using OBFormPost.Application.ViewModel.Posts;
using OBForumPost.Domain.Repository;
using OBForumPost.Domain.Shared;
using System.Threading.Tasks;
using OBFormPost.Application.RequestModel;
using System;

namespace OBFormPost.Application.Service.Post
{
    public sealed class PostControllerService : IPostControllerService
    {
        private readonly IPostRepository repository;

        public PostControllerService(IPostRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IPostViewModel> Get(long postId)
        {
            var post = await repository.Get(postId);
            if (post is null)
            {
                return new PostNotFoundViewModel();
            }
            return PostViewModel.CreateFromPost(post);
        }

        public async Task Create(CreateRequestModel request)
        {
            await repository.Create(new OBForumPost.Domain.Posts.Post
            {
                PostStatus = (PostStatus)request.Status,
                PostedDateTime = DateTimeOffset.Now,
                UpdatedDateTime = DateTimeOffset.Now,
                Title = request.Title,
                Author = new Author
                {
                    Id = request.AuthorId
                }
            });
        }
    }
}
