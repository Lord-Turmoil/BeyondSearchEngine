// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace Tonisoft.AspExtensions.Response;

public class ApiResponse : JsonResult
{
    public ApiResponse(int code, object? value) : base(value)
    {
        StatusCode = code;
    }
}