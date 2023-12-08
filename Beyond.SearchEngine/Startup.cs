using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules;
using Beyond.SearchEngine.Modules.Update.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tonisoft.AspExtensions.Cors;
using Tonisoft.AspExtensions.Module;

namespace BeyondSearchEngine;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        _ConfigureDatabase<BeyondContext>(services);
        services.AddUnitOfWork<BeyondContext>();
        services.RegisterModules();

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
        Configuration.GetRequiredSection(CorsOptions.CorsSection).Bind(corsOptions);
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
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeyondSearch.Api v1"));
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Must be placed between UseRouting and UseEndpoints
        app.UseCors(CorsOptions.CorsPolicyName);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });
    }


    private void _ConfigureDatabase<TContext>(IServiceCollection services) where TContext : DbContext
    {
        string profile = Configuration["Profile"] ?? "Default";
        Console.WriteLine($"   Profile: {profile}");

        string database = Configuration.GetConnectionString("Database") ?? throw new Exception("Missing database");
        string connection = Configuration.GetConnectionString("DefaultConnection") ??
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