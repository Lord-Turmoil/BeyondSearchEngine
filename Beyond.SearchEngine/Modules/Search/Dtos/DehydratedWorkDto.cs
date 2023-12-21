// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Used for search preview.
/// </summary>
public class DehydratedWorkDto
{
    public string Id { get; set; }
    public string Title { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "publication_year")]
    public int PublicationYear { get; set; }
}