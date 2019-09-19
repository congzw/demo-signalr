using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Common.SignalR.Scoped;

namespace ScopedHub
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddSingleton(ScopedConnectionParamParse.Resolve());
            services.AddSingleton<IScopedConnectionRepository, MemoryScopedConnectionRepository>();
            services.AddSingleton<ScopedHubManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var fileServerOptions = new FileServerOptions();
            var defaultPages = new List<string>();
            defaultPages.Add("ScopedHub.html");
            fileServerOptions.DefaultFilesOptions.DefaultFileNames = defaultPages;
            
            app.UseFileServer(fileServerOptions);

            app.UseSignalR(routes =>
            {
                routes.MapHub<Common.SignalR.Scoped.ScopedHub>("/ScopedHub");
            });
        }
    }
}
