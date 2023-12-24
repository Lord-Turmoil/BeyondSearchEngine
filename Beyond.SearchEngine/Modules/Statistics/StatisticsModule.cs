using Beyond.SearchEngine.Modules.Statistics.Services;
using Beyond.SearchEngine.Modules.Statistics.Services.Impl;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Statistics;

public class StatisticsModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IWorkStatisticsService, WorkStatisticsService>();

        return services;
    }
}