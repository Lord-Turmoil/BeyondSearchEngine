﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Impl;

public class InstitutionUpdateImpl
    : GenericUpdateImpl<InstitutionIndexer, Institution, InstitutionDtoBuilder, InstitutionDto>
{
    public InstitutionUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper, logger)
    {
    }
}