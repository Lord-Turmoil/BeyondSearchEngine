using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class SourceDto : OpenAlexStatisticsDto
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
}