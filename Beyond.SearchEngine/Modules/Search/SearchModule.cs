using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Search.Services.Impl;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Search;

public class SearchModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ISimpleSearchService, SimpleSearchService>()
            .AddScoped<IConceptQueryService, ConceptQueryService>();

        return services;
    }
}