// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Wrapper for paged response dto.
/// </summary>
public class PagedDto
{
    public PagedDto(long total, int pageSize, int page, IEnumerable<object> data)
    {
        Total = total;
        PageSize = pageSize;
        Page = page;
        Data = data;
    }

    [JsonProperty(PropertyName = "total")]
    public long Total { get; set; }

    [JsonProperty(PropertyName = "ps")]
    public int PageSize { get; set; }

    [JsonProperty(PropertyName = "p")]
    public int Page { get; set; }

    [JsonProperty(PropertyName = "next")]
    public bool HasNext => Page * PageSize < Total;

    [JsonProperty(PropertyName = "data")]
    public IEnumerable<object> Data { get; set; }
}