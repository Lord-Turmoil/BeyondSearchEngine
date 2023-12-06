﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

namespace Tonisoft.AspExtensions.Module;

public class BaseModule : IModule
{
    public virtual IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }


    public virtual IServiceCollection RegisterModule(IServiceCollection services)
    {
        return services;
    }
}