// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Statistics.Models;
using Beyond.SearchEngine.Modules.Statistics.Services;
using Beyond.SearchEngine.Modules.Statistics.Services.Impl;
using Tonisoft.AspExtensions.Module;

namespace Beyond.SearchEngine.Modules.Statistics;

public class StatisticsModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddCustomRepository<WorkStatistics, WorkStatisticsRepository>()
            .AddCustomRepository<UserLikeRecord, UserLikeRecordRepository>();

        services.AddScoped<IWorkStatisticsService, WorkStatisticsService>();

        return services;
    }
}