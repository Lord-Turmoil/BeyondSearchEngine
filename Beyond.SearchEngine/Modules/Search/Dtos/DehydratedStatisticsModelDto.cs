// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Models;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Used for search preview. Can be used for all models that inherit
///     from OpenAlexStatisticsModel. <see cref="OpenAlexStatisticsModel" />
/// </summary>
public class DehydratedStatisticsModelDto
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "h_index")]
    public int HIndex { get; set; }

    // [JsonProperty(PropertyName = "created")]
    // public DateTime Created { get; set; }
       
    // [JsonProperty(PropertyName = "updated")]
    // public DateTime Updated { get; set; }
}