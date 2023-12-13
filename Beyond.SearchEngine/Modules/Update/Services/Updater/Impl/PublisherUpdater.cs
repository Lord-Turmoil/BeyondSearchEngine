﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models.Elastic;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;
using Nest;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;

public class PublisherUpdater
    : GenericUpdater<PublisherIndexer, ElasticPublisher, PublisherDtoBuilder, PublisherDto>
{
    public PublisherUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger,
        IConfiguration configuration, IElasticClient client)
        : base(unitOfWork, mapper, logger, configuration, client)
    {
    }
}