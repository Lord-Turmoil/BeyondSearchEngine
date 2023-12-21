// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Diagnostics;

namespace Beyond.SearchEngine.Extensions.Middlewares;

/// <summary>
///     Add X-Response-Time header to response.
/// </summary>
public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var watch = new Stopwatch();
        watch.Start();

        context.Response.OnStarting(state => {
            watch.Stop();
            context.Response.Headers["X-Response-Time"] = watch.ElapsedMilliseconds.ToString();
            return Task.CompletedTask;
        }, context);

        await _next(context);
    }
}