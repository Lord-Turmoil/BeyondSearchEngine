using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class DehydratedConceptDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "h_index")]
    public int HIndex { get; set; }
}