// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Extensions.Cache;

public class CacheOptions
{
    public const string CacheSection = "CacheOptions";

    public bool Enable { get; set; }
    public string DefaultConnection { get; set; }
    public string InstanceName { get; set; }

    /// <summary>
    ///     Timeout in minutes.
    /// </summary>
    public int TimeoutInMinutes { get; set; }
}