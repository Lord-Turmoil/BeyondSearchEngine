// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Models.Elastic;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;
using Nest;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;

public class FunderUpdater
    : GenericUpdater<FunderIndexer, ElasticFunder, FunderDtoBuilder, FunderDto>
{
    public FunderUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger,
        IConfiguration configuration, IElasticClient client)
        : base(unitOfWork, mapper, logger, configuration, client)
    {
    }
}