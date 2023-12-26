// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Deprecated.Services;
using Beyond.SearchEngine.Modules.Deprecated.Services.Impl;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Deprecated;

public class DeprecatedModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IDeprecatedService, DeprecatedService>();

        return services;
    }
}