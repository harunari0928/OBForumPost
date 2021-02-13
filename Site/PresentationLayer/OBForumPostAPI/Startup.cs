using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OBFormPost.Application.Service;
using OBFormPost.Application.Service.Post;
using OBFormPost.Application.Service.PostList;
using OBForumPost.Domain.Repository;
using OBForumPost.Persistence.DataBaseContext;
using OBForumPost.Persistence.Repositories;

namespace OBForumAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPostListRepository>( new PostListRepository(new PostContext()));
            services.AddSingleton<IPostRepository>(new PostRepository(new PostContext()));
            services.AddSingleton<IPostListControllerService, PostListControllerService>();
            services.AddSingleton<IPostControllerService, PostControllerService>();
            services.AddControllers();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
