﻿using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Search.Models;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Search;

public class SearchModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        return services;
    }
}