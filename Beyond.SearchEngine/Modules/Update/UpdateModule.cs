using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Update.Models;
using Beyond.SearchEngine.Modules.Update.Services;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Update;

public class UpdateModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddCustomRepository<User, UserRepository>()
            .AddCustomRepository<UpdateHistory, UpdateHistoryRepository>()
            .AddCustomRepository<UpdateConfiguration, UpdateConfigurationRepository>();

        services.AddScoped<IUpdateService, UpdateService>();

        return services;
    }
}