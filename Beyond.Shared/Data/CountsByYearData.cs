using Newtonsoft.Json;

namespace Beyond.Shared.Data;

public class CountsByYearData
{
    public CountsByYearData()
    {
    }

    public CountsByYearData(string data)
    {
        string[] values = data.Split(',');
        Year = int.Parse(values[0]);
        WorksCount = int.Parse(values[1]);
        CitationCount = int.Parse(values[2]);
    }

    [JsonProperty(PropertyName = "year")]
    public int Year { get; set; }

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    public override string ToString()
    {
        return $"{Year},{WorksCount},{CitationCount}";
    }
}