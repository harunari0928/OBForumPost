using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OBForumPost.Domain.Repository;
using OBForumPost.Domain.Shared;
using OBForumPost.Persistence.DataBaseContext;
using OBForumPost.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OBForumPost.Persistence.Test.Repositories
{
    public abstract class PostListRepositoryTest
    {
        protected PostListRepositoryTest(DbContextOptions<PostContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        protected IReadOnlyList<PostStoredDb> DummyData { get; private set; }

        protected DbContextOptions<PostContext> ContextOptions { get; }

        private void Seed()
        {
            using var context = new PostContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            DummyData = GetDummyPostData().ToList();
            context.AddRange(DummyData);

            context.SaveChanges();
        }

        private static IEnumerable<PostStoredDb> GetDummyPostData()
        {
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2019/12/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2020/2/24 13:00:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test1",
                AuthorId = 4
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2014/10/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2015/3/6 13:00:00 +9:00"),
                PostStatus = (int)PostStatus.Open,
                Title = "test2",
                AuthorId = 4
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2013/12/23 4:50:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2017/2/24 21:30:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test3",
                AuthorId = 10
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2004/4/4 4:44:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2006/9/24 9:30:00 +9:00"),
                PostStatus = (int)PostStatus.Open,
                Title = "test4",
                AuthorId = 11
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2041/12/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2055/2/24 15:30:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test5",
                AuthorId = 12
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2016/12/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2017/2/24 15:30:00 +9:00"),
                PostStatus = (int)PostStatus.Open,
                Title = "test6",
                AuthorId = 13
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2018/12/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2021/2/24 15:30:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test7",
                AuthorId = 10
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2021/12/23 14:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2022/2/24 15:30:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test8",
                AuthorId = 10
            };
            yield return new PostStoredDb
            {
                PostedDateTime = DateTimeOffset.Parse("2023/7/21 5:00:00 +9:00"),
                UpdatedDateTime = DateTimeOffset.Parse("2024/5/5 5:14:00 +9:00"),
                PostStatus = (int)PostStatus.Close,
                Title = "test9",
                AuthorId = 10
            };
        }

        [Fact]
        public async Task 引数が不正な値の場合例外を吐くこと()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);
            await Assert.ThrowsAsync<ArgumentException>(() => repository.Get(0, 0));
            await Assert.ThrowsAsync<ArgumentException>(() => repository.Get(0, 1));
            await Assert.ThrowsAsync<ArgumentException>(() => repository.Get(1, 0));
            await Assert.ThrowsAsync<ArgumentException>(() => repository.Get(1, -1));
        }

        [Fact]
        public async Task 指定したページ及びサイズの投稿を取得できること_1ページ3投稿存在する場合()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);

            {
                var page = 1;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 2;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 3;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }
        }

        [Fact]
        public async Task 指定したページ及びサイズの投稿を取得できること_1ページ4投稿存在する場合()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);

            {
                var page = 1;
                var pageSize = 4;
                var actuals = await repository.Get(page, pageSize);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 2;
                var pageSize = 4;
                var actuals = await repository.Get(page, pageSize);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 3;
                var pageSize = 4;
                var actuals = await repository.Get(page, pageSize);

                Assert.Single(actuals);

                foreach (var (dummy, actual) in DummyData.Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }
        }

        [Fact]
        public async Task 実際のページよりも大きい値を指定したとき取得件数は0件であること()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);
            var page = 4;
            var pageSize = 3;

            Assert.Empty(await repository.Get(page, pageSize));
        }

        [Fact]
        public async Task 投稿リスト全体を投稿日時昇順でソートして投稿を返せること_1ページ3投稿存在する場合()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);

            {
                var page = 1;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.PostedDateTimeAsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderBy(x => x.PostedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 2;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.PostedDateTimeAsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderBy(x => x.PostedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 3;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.PostedDateTimeAsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderBy(x => x.PostedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }
        }

        [Fact]
        public async Task 投稿リスト全体を投稿日時降順でソートして投稿を返せること_1ページ3投稿存在する場合()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);

            {
                var page = 1;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.PostedDateTimeDsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderByDescending(x => x.PostedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 2;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.PostedDateTimeDsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderByDescending(x => x.PostedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 3;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.PostedDateTimeDsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderByDescending(x => x.PostedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }
        }

        [Fact]
        public async Task 投稿リスト全体を投稿更新日時昇順でソートして投稿を返せること_1ページ3投稿存在する場合()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);

            {
                var page = 1;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.UpdatedDateTimeAsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderBy(x => x.UpdatedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 2;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.UpdatedDateTimeAsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderBy(x => x.UpdatedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 3;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.UpdatedDateTimeAsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderBy(x => x.UpdatedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }
        }

        [Fact]
        public async Task 投稿リスト全体を投稿更新日時降順でソートして投稿を返せること_1ページ3投稿存在する場合()
        {
            using var context = new PostContext(ContextOptions);
            var repository = new PostListRepository(context);

            {
                var page = 1;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.UpdatedDateTimeDsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderByDescending(x => x.UpdatedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 2;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.UpdatedDateTimeDsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderByDescending(x => x.UpdatedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }

            {
                var page = 3;
                var pageSize = 3;
                var actuals = await repository.Get(page, pageSize, OrderByOptions.UpdatedDateTimeDsc);

                Assert.Equal(pageSize, actuals.Count);

                foreach (var (dummy, actual) in DummyData
                    .OrderByDescending(x => x.UpdatedDateTime)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .Zip(actuals, (d, a) => (d, a)))
                {
                    Assert.Equal(dummy.Title, actual.Title);
                    Assert.Equal(dummy.PostStatus, (int)actual.PostStatus);
                    Assert.Equal(dummy.PostedDateTime, actual.PostedDateTime);
                    Assert.Equal(dummy.UpdatedDateTime, actual.UpdatedDateTime);
                }
            }
        }
    }

    /// <summary>
    /// SQLiteのインメモリデータベースでテストする
    /// </summary>
    public class SqlitePostListRepositoryTest : PostListRepositoryTest, IDisposable
    {
        private readonly DbConnection connection;

        public SqlitePostListRepositoryTest()
            : base(new DbContextOptionsBuilder<PostContext>()
                .UseSqlite(CreateInMemoryDatabase())
                .Options)
        {
            connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => connection.Dispose();
    }
}
