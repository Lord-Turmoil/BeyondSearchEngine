// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Tonisoft.AspExtensions.Response;

// Dto that does have data
public class OrdinaryDto : ApiResponseDto
{
    public OrdinaryDto(int status, string? message = null) : base(status, message)
    {
    }
}

public class BadDto : ApiResponseDto
{
    public BadDto(int status, string? message = "Bad request", object? data = null) : base(status, message, data)
    {
    }
}

public class OkDto : ApiResponseDto
{
    public OkDto(string? message = "Nice request", object? data = null) : base(0, message, data)
    {
    }
}

public class OkWithDataDto : OkDto
{
    public OkWithDataDto(object? data = null) : base("Nice request", data)
    {
    }
}

public class BadRequestDto : ApiResponseDto
{
    public BadRequestDto(string? message = "Request format error", object? data = null)
        : base(StatusCodes.Status400BadRequest, message, data)
    {
    }
}

public class UnauthorizedDto : ApiResponseDto
{
    public UnauthorizedDto(string? message = "Who are you?", object? data = null)
        : base(StatusCodes.Status401Unauthorized, message, data)
    {
    }
}

public class ForbiddenDto : ApiResponseDto
{
    public ForbiddenDto(string? message = "You are not allowed to do this", object? data = null)
        : base(StatusCodes.Status403Forbidden, message, data)
    {
    }
}

public class NotFoundDto : ApiResponseDto
{
    public NotFoundDto(string? message = "Not found", object? data = null)
        : base(StatusCodes.Status404NotFound, message, data)
    {
    }
}

public class MethodNotAllowedDto : ApiResponseDto
{
    public MethodNotAllowedDto(string? message = "Method not allowed", object? data = null)
        : base(StatusCodes.Status405MethodNotAllowed, message, data)
    {
    }
}

public class InternalServerErrorDto : ApiResponseDto
{
    public InternalServerErrorDto(string? message = "Unexpected error", object? data = null)
        : base(StatusCodes.Status500InternalServerError, message, data)
    {
    }
}