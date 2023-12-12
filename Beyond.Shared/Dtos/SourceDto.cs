using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class SourceDto : ElasticDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    [JsonProperty(PropertyName = "homepage_url")]
    public string HomepageUrl { get; set; }


    /***              Relations               ***/

    [JsonProperty(PropertyName = "host_id")]
    public string HostId { get; set; }

    [JsonProperty(PropertyName = "host_name")]
    public string HostName { get; set; }

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));


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
}