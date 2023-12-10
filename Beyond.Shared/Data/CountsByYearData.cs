using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    public static CountsByYearData Build(JObject json)
    {
        return new CountsByYearData {
            Year = json["year"].ToIntNotNull("year", 0),
            WorksCount = json["works_count"].ToIntNotNull("works_count", 0),
            CitationCount = json["citation_count"].ToIntNotNull("citation_count", 0)
        };
    }
}