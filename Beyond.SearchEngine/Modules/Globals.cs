// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Nest;

namespace Beyond.SearchEngine.Modules;

public static class Globals
{
    public const int DefaultPageSize = 20;
    public const int DefaultPage = 0;
    public const int MaxPageSize = 100;
    public const int MaxPagePressure = 1000;

    public static readonly HashSet<string> AvailableTypes = [
        "authors",
        "concepts",
        "funders",
        "institutions",
        "publishers",
        "sources",
        "works"
    ];

    public static readonly HashSet<string> AvailableSortFiled = [
        "title",
        "citation",
        "time"
    ];

    public static readonly Fuzziness DefaultFuzziness = Fuzziness.EditDistance(1);

    public static TimeSpan DefaultCacheTimeout = TimeSpan.FromDays(1);
}