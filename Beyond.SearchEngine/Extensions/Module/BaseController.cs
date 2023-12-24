// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace Tonisoft.AspExtensions.Module;

public class BaseController<TController> : Controller where TController : Controller
{
    protected readonly ILogger<TController> _logger;

    protected BaseController(ILogger<TController> logger)
    {
        _logger = logger;
    }
}