using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Statistics.Dtos;

public class WorkStatisticsDto
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "likes")]
    public int Likes { get; set; }

    [JsonProperty(PropertyName = "questions")]
    public int Questions { get; set; }

    [JsonProperty(PropertyName = "views")]
    public int Views { get; set; }
}