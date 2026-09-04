using House.Objects;
using House.Objects.Attributes;

namespace House.API
{
    using System;
    using System.CodeDom.Compiler;
    using BackgroundService;
    using DAL;
    using HLL;
    using HLL.Dashboard.Bindicator;
    using HLL.Dashboard.Bindicator.Models;
    using HLL.Dashboard.WeatherFeed.Models;
    using HLL.News.Models;
    using HLL.TerrariaRunner;
    using HLL.TerrariaRunner.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NSwag;
    using Scrutor;
    using SignalR;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(cfg =>
            {
                cfg.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            });

            services.AddOpenApiDocument(c =>
            {
                c.Title = "House API";
                c.Description = "An api for the house";
                c.DocumentProcessors.Add(new SchemaExtenderDocumentProcessor());
            });

            services.AddLazyCache();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .AllowCredentials()
                        .WithOrigins("http://localhost", 
                                     "https://localhost", 
                                     "https://localhost:44359", 
                                     "https://localhost:44370",
                                     "http://localhost:62110/",
                                     "http://192.168.0.69:420",
                                     "http://localhost:44370",
                                     "http://192.168.1.100",
                                     "http://192.168.1.69");
                });
            });

            services.AddSignalR(config =>
            {
                config.ClientTimeoutInterval = TimeSpan.FromSeconds(120);
                config.EnableDetailedErrors = true;
            });

            services.Configure<Lookup>(option => Configuration.GetSection("Lookup").Bind(option));
            services.Configure<ConnectionStrings>(option => Configuration.GetSection("ConnectionStrings").Bind(option));
            services.Configure<OpenWeatherApi>(option => Configuration.GetSection("OpenWeatherApi").Bind(option));
            services.Configure<NewsApi>(option => Configuration.GetSection("NewsApi").Bind(option));
            services.Configure<DbConnections>(option => Configuration.GetSection("DbConnections").Bind(option));
            services.Configure<TerrariaConfig>(option => Configuration.GetSection(nameof(TerrariaConfig)).Bind(option));

            services.AddSingleton<ITerrariaRunner, TerrariaRunner>();

            services.AddHostedService<NotificationHostedService>();

            ScanForAllRemainingRegistrations(services);
        }

        public static void ScanForAllRemainingRegistrations(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(Startup), typeof(BindicatorProvider), typeof(BaseRepository))
                .AddClasses(x => x.WithoutAttribute(typeof(GeneratedCodeAttribute)).WithoutAttribute<ScrutorIgnoreAttribute>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime()); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("SiteCorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();

            app.UseSwaggerUi();

            app.UseReDoc(cfg =>
            {
                cfg.Path = "/docs";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AppHub>("/app-hub");
            });
        }
    }
}
