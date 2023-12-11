﻿using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

/// <summary>
///     Complete author information.
/// </summary>
public class AuthorDto : ElasticDto
{
    [JsonProperty(PropertyName = "orcid")]
    public string OrcId { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /***              Statistics               ***/

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "h_index")]
    public int HIndex { get; set; }

    [JsonProperty(PropertyName = "counts_by_year")]
    public List<CountsByYearData> CountsByYearList { get; set; }

    [JsonIgnore]
    public string CountsByYears => string.Join(";", CountsByYearList.Select(c => c.ToString()));

    /***                Relation               ***/

    [JsonProperty(PropertyName = "institution")]
    public InstitutionData? InstitutionData { get; set; }

    [JsonIgnore]
    public string Institution => InstitutionData?.ToString() ?? "";

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));
}