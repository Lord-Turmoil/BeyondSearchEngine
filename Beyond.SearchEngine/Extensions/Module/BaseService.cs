// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;

namespace Tonisoft.AspExtensions.Module;

public class BaseService<TService>
{
    protected readonly ILogger<TService> _logger;
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
}