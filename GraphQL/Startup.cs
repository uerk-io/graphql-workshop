using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Types;
using GraphQL.Speakers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConferencePlanner.GraphQL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
/*            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite("Data Source=conferences.db"));*/

            services.AddPooledDbContextFactory<ApplicationDbContext>((provider, optionsBuilder) =>
            {
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("ConferencePlanner"), contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly("GraphQL"));
            });

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType(d => d.Name("Mutation")).AddTypeExtension<SpeakerMutations>()
                .AddType<SpeakerType>()
                .EnableRelaySupport()
                .AddDataLoader<SpeakerByIdDataLoader>()
                .AddDataLoader<SessionByIdDataLoader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}