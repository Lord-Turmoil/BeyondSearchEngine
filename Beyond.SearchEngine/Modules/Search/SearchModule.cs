using Beyond.SearchEngine.Modules.Search.Services;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Search;

public class SearchModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ISimpleSearchService, SimpleSearchService>();

        return services;
    }
}