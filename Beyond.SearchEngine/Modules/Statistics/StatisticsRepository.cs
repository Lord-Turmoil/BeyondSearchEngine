// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Arch.EntityFrameworkCore.UnitOfWork;
using Beyond.SearchEngine.Modules.Statistics.Models;

namespace Beyond.SearchEngine.Modules.Statistics;

public class WorkStatisticsRepository : Repository<WorkStatistics>
{
    public WorkStatisticsRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}

public class UserLikeRecordRepository : Repository<UserLikeRecord>
{
    public UserLikeRecordRepository(BeyondContext dbContext) : base(dbContext)
    {
    }
}