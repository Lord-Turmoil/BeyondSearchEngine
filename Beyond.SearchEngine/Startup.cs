using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Elastic;
using Beyond.SearchEngine.Extensions.Middlewares;
using Beyond.SearchEngine.Modules;
using Beyond.SearchEngine.Modules.Update.Services;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nest;
using Tonisoft.AspExtensions.Cors;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        _ConfigureDatabase<BeyondContext>(services);
        services.AddUnitOfWork<BeyondContext>();
        services.RegisterModules();

        // Version
        Globals.Version = _configuration["Version"] ?? Globals.Version;

        // Controllers
        services.AddControllers().AddNewtonsoftJson();

        // Swagger service
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeyondSearch.Api", Version = "v1" });
        });

        // AutoMapper
        var autoMapperConfig = new MapperConfiguration(config => { config.AddProfile(new AutoMapperProfile()); });
        services.AddSingleton(autoMapperConfig.CreateMapper());

        // CORS
        var corsOptions = new CorsOptions();
        _configuration.GetRequiredSection(CorsOptions.CorsSection).Bind(corsOptions);
        if (corsOptions.Enable)
        {
            services.AddCors(options => {
                options.AddPolicy(
                    CorsOptions.CorsPolicyName,
                    policy => {
                        if (corsOptions.AllowAny)
                        {
                            policy.AllowAnyOrigin();
                        }
                        else
                        {
                            foreach (string origin in corsOptions.Origins)
                            {
                                policy.WithOrigins(origin);
                            }

                            policy.AllowCredentials();
                        }

                        policy.AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        // Elasticsearch
        var elasticOptions = new ElasticOptions();
        _configuration.GetRequiredSection(ElasticOptions.ElasticSection).Bind(elasticOptions);
        var pool = new SingleNodeConnectionPool(new Uri(elasticOptions.DefaultConnection));
        ConnectionSettings settings = new ConnectionSettings(pool)
            .EnableHttpPipelining()
            .DisableDirectStreaming()
            .EnableApiVersioningHeader();
        if (elasticOptions.EnableBasicAuth)
        {
            settings.BasicAuthentication(elasticOptions.Username, elasticOptions.Password);
        }

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        // Cache
        var cacheOptions = new CacheOptions();
        _configuration.GetRequiredSection(CacheOptions.CacheSection).Bind(cacheOptions);
        if (cacheOptions.Enable)
        {
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = cacheOptions.DefaultConnection;
                options.InstanceName = cacheOptions.InstanceName;
            });
            services.AddSingleton<ICacheAdapter, RedisCacheAdapter>();
            Globals.DefaultCacheTimeout = TimeSpan.FromMinutes(cacheOptions.TimeoutInMinutes);
        }
        else
        {
            services.AddSingleton<ICacheAdapter, NoCacheAdapter>();
        }

        // Background task
        services.AddSingleton<UpdateTask>();
        services.AddHostedService<UpdateTask>(provider
            => provider.GetRequiredService<UpdateTask>());
    }


    /// <summary>
    ///     For middleware order, please refer to:
    ///     https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/index/_static/middleware-pipeline.svg?view=aspnetcore-6.0
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeyondSearch.Api v1"));
        }

        // app.UseHttpsRedirection();
        // app.UseStaticFiles();
        app.UseRouting();

        // Must be placed between UseRouting and UseEndpoints
        app.UseCors(CorsOptions.CorsPolicyName);

        // app.UseAuthentication();
        // app.UseAuthorization();

        // Must be placed before UseEndpoints.
        app.UseMiddleware<ResponseTimeMiddleware>();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });

        // loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\BeyondSearch.Api.log");
    }


    private void _ConfigureDatabase<TContext>(IServiceCollection services) where TContext : DbContext
    {
        string profile = _configuration["Profile"] ?? "Default";
        Console.WriteLine($"   Profile: {profile}");

        string database = _configuration.GetConnectionString("Database") ?? throw new Exception("Missing database");
        string connection = _configuration.GetConnectionString("DefaultConnection") ??
                            throw new Exception("Missing database connection");

        Console.WriteLine($"  Database: {database}");
        Console.WriteLine($"Connection: {connection}");

        switch (database)
        {
            case "MySQL":
                services.AddDbContext<TContext>(option => { option.UseMySQL(connection); });
                break;
            default:
                throw new Exception($"Invalid database: {database}");
        }
    }
}