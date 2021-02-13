using OBForumPost.Domain.Posts;
using OBForumPost.Domain.Repository;
using OBForumPost.Persistence.DataBaseContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OBForumPost.Persistence.Repositories
{
    public sealed class PostRepository : IPostRepository
    {
        private readonly PostContext db;
        public PostRepository(PostContext db)
        {
            this.db = db;
        }

        public async Task<Post> Create(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title))
            {
                throw new ArgumentException("タイトルを指定してください", nameof(post));
            }
            if (post.PostedDateTime != post.UpdatedDateTime)
            {
                throw new ArgumentException("投稿日時と投稿更新日時を同じにしてください", nameof(post));
            }
            if (post.Author.Id < 1)
            {
                throw new ArgumentException("投稿者Idを1以上で指定してください", nameof(post));
            }

            var entity = new PostEntity
            {
                PostedDateTime = post.PostedDateTime,
                UpdatedDateTime = post.UpdatedDateTime,
                PostStatus = (int)post.PostStatus,
                Title = post.Title,
                AuthorId = post.Author.Id
            };
            await db.AddAsync(entity);
            await db.SaveChangesAsync();

            return new Post
            {
                Id = entity.PostId,
                PostedDateTime = post.PostedDateTime,
                UpdatedDateTime = post.UpdatedDateTime,
                PostStatus = post.PostStatus,
                Title = post.Title,
                Author = new Domain.Shared.Author
                {
                    Id = post.Author.Id,
                    Name = post.Author.Name
                }
            };
        }

        public async Task<Post?> Get(long id)
        {
            if (id < 1)
            {
                throw new ArgumentException("1以上の値を指定してください", nameof(id));
            }

            var singleDataRow = await Task.Run(() => db.Posts.FirstOrDefault(x => x.PostId == id));
            return singleDataRow is null ? null : new Post
            {
                Id = singleDataRow.PostId,
                PostedDateTime = singleDataRow.PostedDateTime,
                UpdatedDateTime = singleDataRow.UpdatedDateTime,
                PostStatus = (Domain.Shared.PostStatus)singleDataRow.PostStatus,
                Title = singleDataRow.Title,
                Author = new Domain.Shared.Author
                {
                    Id = singleDataRow.AuthorId
                }
            };
        }

        public async Task Update(Post post)
        {
            if (post.Id < 1)
            {
                throw new ArgumentException("Idは1以上の値を指定してください", nameof(post));
            }
            if (string.IsNullOrWhiteSpace(post.Title))
            {
                throw new ArgumentException("タイトルを指定してください", nameof(post));
            }
            if (post.Author.Id < 1)
            {
                throw new ArgumentException("投稿者Idを1以上で指定してください", nameof(post));
            }

            var updatingPostEntity = db.Posts.FirstOrDefault(x => x.PostId == post.Id);
            if (updatingPostEntity is null)
            {
                return;
            }

            updatingPostEntity.UpdatedDateTime = DateTimeOffset.Now;
            updatingPostEntity.PostStatus = (int)post.PostStatus;
            updatingPostEntity.Title = post.Title;
            updatingPostEntity.AuthorId = post.Author.Id;
            db.Posts.Update(updatingPostEntity);
            await db.SaveChangesAsync();
        }

        public async Task Remove(long id)
        {
            if (id < 1)
            {
                throw new ArgumentException("1以上の値を指定してください", nameof(id));
            }

            var entity = db.Posts.FirstOrDefault(x => x.PostId == id);
            if (entity is null) { return; }

            db.Remove(entity);
            await db.SaveChangesAsync();
        }
    }
}
