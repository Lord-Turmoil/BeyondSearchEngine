// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Tonisoft.AspExtensions.Module;

public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection services);
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}

public static class ModuleExtensions
{
    // this could also be added into the DI container
    private static readonly List<IModule> registeredModules = new();


    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        IEnumerable<IModule> modules = DiscoverModules();
        foreach (IModule module in modules)
        {
            module.RegisterModule(services);
            registeredModules.Add(module);
        }

        return services;
    }


    public static WebApplication MapEndpoints(this WebApplication app)
    {
        foreach (IModule module in registeredModules)
        {
            module.MapEndpoints(app);
        }

        return app;
    }


    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}