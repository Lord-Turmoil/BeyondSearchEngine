using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class ElasticDto
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string FullId => "https://openalex.org/" + Id;


    [JsonProperty(PropertyName = "created")]
    public DateTime Created { get; set; }

    [JsonProperty(PropertyName = "updated")]
    public DateTime Updated { get; set; }
}