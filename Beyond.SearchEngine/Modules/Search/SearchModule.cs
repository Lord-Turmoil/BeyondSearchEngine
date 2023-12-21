// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Search.Services.Impl;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Search;

public class SearchModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ISimpleSearchService, SimpleSearchService>()
            .AddScoped<IConceptQueryService, ConceptQueryService>()
            .AddScoped<ISourceQueryService, SourceQueryService>()
            .AddScoped<IWorkQueryService, WorkQueryService>();

        return services;
    }
}