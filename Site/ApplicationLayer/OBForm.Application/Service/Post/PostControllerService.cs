using OBFormPost.Application.ViewModel.Posts;
using OBForumPost.Domain.Repository;
using OBForumPost.Domain.Shared;
using System;
using System.Threading.Tasks;

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

        public async Task<PostViewModel> Create(CreateRequestModel request)
        {
            var post = await repository.Create(new OBForumPost.Domain.Posts.Post
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
            return PostViewModel.CreateFromPost(post);
        }

        public async Task<IPostViewModel> Update(PostViewModel originalPost)
        {
            var post = new OBForumPost.Domain.Posts.Post
            {
                Id = originalPost.Id,
                PostStatus = originalPost.Status,
                PostedDateTime = originalPost.PostedDateTime,
                UpdatedDateTime = DateTimeOffset.Now,
                Title = originalPost.Title
            };
            await repository.Update(post);
            var updatedPost = await repository.Get(post.Id);
            if (updatedPost is null)
            {
                return new PostNotFoundViewModel();
            }
            return PostViewModel.CreateFromPost(updatedPost);
        }

        public async Task Remove(long postId)
        {
            await repository.Remove(postId);
        }
    }
}
