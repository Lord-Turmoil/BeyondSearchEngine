﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;

public class WorkUpdater
    : GenericUpdater<WorkIndexer, Work, WorkDtoBuilder, WorkDto>
{ 
    public WorkUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger, IConfiguration configuration)
        : base(unitOfWork, mapper, logger, configuration)
    {
    }
}