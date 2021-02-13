using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Repository;
using OBForumPost.Persistence.DataBaseContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OBForumPost.Persistence.Repositories
{
    public sealed class PostListRepository : IPostListRepository
    {
        private readonly PostContext db;
        public PostListRepository(PostContext db)
        {
            this.db = db;
        }

        public Task<PostList> Get(int page, int pageSize, OrderByOptions orderByOption = OrderByOptions.None)
        {
            if (page < 1)
            {
                throw new ArgumentException("1以上を指定してください", nameof(page));
            }
            if (pageSize < 1)
            {
                throw new ArgumentException("1以上を指定してください", nameof(pageSize));
            }

            return Task.Run(() =>
            {
                var dataRows = db
                .GetOrderedPostsQuery(orderByOption)
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
                var posts = dataRows.Select(x => new Post
                {
                    Id = x.PostId,
                    PostedDateTime = x.PostedDateTime,
                    UpdatedDateTime = x.UpdatedDateTime,
                    PostStatus = (Domain.Shared.PostStatus)x.PostStatus,
                    Title = x.Title,
                    Author = new Domain.Shared.Author()
                });
                return new PostList(posts);
            });
        }
    }
}
