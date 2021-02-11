using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OBForumPost.Domain.Posts;
using OBForumPost.Domain.Shared;
using OBForumPost.Persistence.DataBaseContext;
using OBForumPost.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OBForumPost.Persistence.Test.Repositories
{
    public sealed class SqlitePostRepositoryTest
    {
        public sealed class GetTest : TestUsingSqlite
        {
            private readonly PostEntity dummyData = new PostEntity // ID: 1の投稿のみ作成される
            {
                PostedDateTime = DateTimeOffset.Parse("2019/12/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2020/2/24 13:00:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test11",
                AuthorId = 4
            };

            [Fact]
            public async Task 不正なIDを引数で渡したとき例外を吐くこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Get(0));
            }

            [Fact]
            public async Task 指定したIDの投稿が存在する場合その投稿のオブジェクトを返せすこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                var actual = await repository.Get(1);
                Assert.Equal(dummyData.PostId, actual.Id);
                Assert.Equal(dummyData.PostedDateTime, actual.PostedDateTime);
                Assert.Equal(dummyData.UpdatedDateTime, actual.UpdatedDateTime);
                Assert.Equal(dummyData.Title, actual.Title);
            }

            [Fact]
            public async Task 指定したIDの投稿が存在しない場合nullを返すこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                Assert.Null(await repository.Get(int.MaxValue));
            }

            protected override void Seed(PostContext postContext)
            {
                postContext.Add(dummyData);
            }
        }
        public sealed class CreateTest : TestUsingSqlite
        {
            [Fact]
            public async Task 引数の投稿が不正な場合例外を吐くこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Create(new Post
                {
                    PostedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    PostStatus = PostStatus.Open,
                    Title = "　", // タイトルが空
                    Author = new Author
                    {
                        Id = 1,
                        Name = "aaa"
                    }
                }));

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Create(new Post
                {
                    PostedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:01 +9:00"), // 新規作成なのに投稿日時と投稿更新日時が異なる
                    PostStatus = PostStatus.Open,
                    Title = "タイトル",
                    Author = new Author
                    {
                        Id = 1,
                        Name = "aaa"
                    }
                }));
                await Assert.ThrowsAsync<ArgumentException>(() => repository.Create(new Post
                {
                    PostedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    PostStatus = PostStatus.Open,
                    Title = "あああ",
                    Author = new Author
                    {
                        Id = 0, // 投稿者Idが不正
                        Name = "aaa"
                    }
                }));
            }

            [Fact]
            public async Task 想定した投稿を作成できること()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                var expected = new Post
                {
                    PostedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/02/11 11:00:00 +9:00"),
                    PostStatus = PostStatus.Open,
                    Title = "taitoru",
                    Author = new Author
                    {
                        Id = 14,
                        Name = "なまえ"
                    }
                };
                var createdPost = await repository.Create(expected);

                Assert.Equal(expected.PostedDateTime, createdPost.PostedDateTime);
                Assert.Equal(expected.UpdatedDateTime, createdPost.UpdatedDateTime);
                Assert.Equal(expected.PostStatus, createdPost.PostStatus);
                Assert.Equal(expected.Title, createdPost.Title);
                Assert.Equal(expected.Author.Id, createdPost.Author.Id);

                // 実際のDBの値のテスト
                var storedData = context.Posts.Single(x => x.PostId == createdPost.Id);
                Assert.Equal(expected.PostedDateTime, storedData.PostedDateTime);
                Assert.Equal(expected.UpdatedDateTime, storedData.UpdatedDateTime);
                Assert.Equal(expected.PostStatus, (PostStatus)storedData.PostStatus);
                Assert.Equal(expected.Title, storedData.Title);
                Assert.Equal(expected.Author.Id, storedData.AuthorId);
            }

            protected override void Seed(PostContext _) { }
        }
        public sealed class UpdateTest : TestUsingSqlite
        {
            private readonly IReadOnlyList<PostEntity> dummyData = GetDummyPostData().ToList();

            [Fact]
            public async Task 不正なIDを引数で渡したとき例外を吐くこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Update(new Post
                {
                    Id = 0, // Idが不正
                    Title = "test",
                    Author = new Author
                    {
                        Id = 222,
                    }
                }));

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Update(new Post
                {
                    Id = 1,
                    Title = " ", // タイトルが不正
                    Author = new Author
                    {
                        Id = 222,
                    }
                }));

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Update(new Post
                {
                    Id = 1,
                    Title = "test",
                    Author = new Author
                    {
                        Id = 0, // 投稿者IDが不正
                    }
                }));
            }

            [Fact]
            public async Task 指定したIDの投稿を更新できること()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                var postBeforeUpdate = await repository.Get(dummyData[2].PostId);
                var updateRequest = new Post
                {
                    Id = postBeforeUpdate.Id,
                    PostedDateTime = postBeforeUpdate.PostedDateTime,
                    UpdatedDateTime = postBeforeUpdate.UpdatedDateTime,
                    PostStatus = PostStatus.Open,
                    Title = "更新後",
                    Author = new Author
                    {
                        Id = int.MaxValue,
                        Name = postBeforeUpdate.Author.Name
                    }
                };
                await repository.Update(updateRequest);

                var postEntityAfterUpdate = context.Posts.Single(x => x.PostId == postBeforeUpdate.Id);

                Assert.NotEqual(postBeforeUpdate.UpdatedDateTime, postEntityAfterUpdate.UpdatedDateTime); // 記事更新日時が更新されていること
                Assert.Equal(updateRequest.PostStatus, (PostStatus)postEntityAfterUpdate.PostStatus);
                Assert.Equal(updateRequest.Title, postEntityAfterUpdate.Title);
                Assert.Equal(updateRequest.Author.Id, postEntityAfterUpdate.AuthorId);
            }

            [Fact]
            public async Task 記事投稿日時は更新されないこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                var postBeforeUpdate = await repository.Get(dummyData[2].PostId);
                var updateRequest = new Post
                {
                    Id = postBeforeUpdate.Id,
                    PostedDateTime = DateTimeOffset.MaxValue,
                    UpdatedDateTime = postBeforeUpdate.UpdatedDateTime,
                    PostStatus = PostStatus.Open,
                    Title = "更新後",
                    Author = new Author
                    {
                        Id = int.MaxValue,
                        Name = postBeforeUpdate.Author.Name
                    }
                };
                await repository.Update(updateRequest);

                var postEntityAfterUpdate = context.Posts.Single(x => x.PostId == postBeforeUpdate.Id);
                Assert.Equal(postBeforeUpdate.PostedDateTime, postEntityAfterUpdate.PostedDateTime);
            }

            [Fact]
            public async Task 存在しないIDの投稿を更新しようとしたとき何も起きないこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                await repository.Update(new Post
                {
                    Id = int.MaxValue,
                    Title = "aaa",
                    Author = new Author
                    {
                        Id = 222,
                    }
                });
            }

            protected override void Seed(PostContext postContext)
            {
                postContext.AddRange(dummyData);
            }
        }
        public sealed class RemoveTest : TestUsingSqlite
        {
            private readonly IReadOnlyList<PostEntity> dummyData = GetDummyPostData().ToList();

            [Fact]
            public async Task 不正なIDを引数で渡したとき例外を吐くこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                await Assert.ThrowsAsync<ArgumentException>(() => repository.Remove(0));
            }

            [Fact]
            public async Task 指定したIDの投稿を削除できること()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                var removingId = dummyData[2].PostId;
                await repository.Remove(removingId);

                Assert.Null(context.Posts.FirstOrDefault(x => x.PostId == removingId));
            }

            [Fact]
            public async Task 存在しないIDの投稿を削除しようとしたときデータは何も消されないこと()
            {
                using var context = new PostContext(contextOptions);
                var repository = new PostRepository(context);

                await repository.Remove(int.MaxValue);

                Assert.Equal(dummyData.Count, context.Posts.Count());
            }

            protected override void Seed(PostContext postContext)
            {
                postContext.AddRange(dummyData);
            }
        }
        public abstract class TestUsingSqlite : IDisposable
        {
            private readonly DbConnection connection;
            protected readonly DbContextOptions<PostContext> contextOptions;

            public TestUsingSqlite()
            {
                contextOptions = new DbContextOptionsBuilder<PostContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options;
                connection = RelationalOptionsExtension.Extract(contextOptions).Connection;

                InitializeDatabase();
            }

            private static DbConnection CreateInMemoryDatabase()
            {
                var connection = new SqliteConnection("Filename=:memory:");

                connection.Open();

                return connection;
            }

            public void Dispose()
            {
                connection.Dispose();
                GC.SuppressFinalize(this);
            }

            private void InitializeDatabase()
            {
                using var context = new PostContext(contextOptions);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Seed(context);

                context.SaveChanges();
            }

            protected abstract void Seed(PostContext postContext);

            protected static IEnumerable<PostEntity> GetDummyPostData()
            {
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2019/12/23 14:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2020/2/24 13:00:00 +9:00"),
                    PostStatus = (int)PostStatus.Close,
                    Title = "test1",
                    AuthorId = 4
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2014/10/23 14:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2015/3/6 13:00:00 +9:00"),
                    PostStatus = (int)PostStatus.Open,
                    Title = "test2",
                    AuthorId = 4
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2013/12/23 4:50:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2017/2/24 21:30:00 +9:00"),
                    PostStatus = (int)PostStatus.Close,
                    Title = "test3",
                    AuthorId = 10
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2004/4/4 4:44:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2006/9/24 9:30:00 +9:00"),
                    PostStatus = (int)PostStatus.Open,
                    Title = "test4",
                    AuthorId = 11
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2041/12/23 14:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2055/2/24 15:30:00 +9:00"),
                    PostStatus = (int)PostStatus.Close,
                    Title = "test5",
                    AuthorId = 12
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2016/12/23 14:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2017/2/24 15:30:00 +9:00"),
                    PostStatus = (int)PostStatus.Open,
                    Title = "test6",
                    AuthorId = 13
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2018/12/23 14:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/2/24 15:30:00 +9:00"),
                    PostStatus = (int)PostStatus.Close,
                    Title = "test7",
                    AuthorId = 10
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2021/12/23 14:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2022/2/24 15:30:00 +9:00"),
                    PostStatus = (int)PostStatus.Close,
                    Title = "test8",
                    AuthorId = 10
                };
                yield return new PostEntity
                {
                    PostedDateTime = DateTimeOffset.Parse("2023/7/21 5:00:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2024/5/5 5:14:00 +9:00"),
                    PostStatus = (int)PostStatus.Close,
                    Title = "test9",
                    AuthorId = 10
                };
            }
        }
    }
}
