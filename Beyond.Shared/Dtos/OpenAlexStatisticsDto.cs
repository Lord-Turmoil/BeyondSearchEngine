using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class OpenAlexStatisticsDto : OpenAlexDto
{
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