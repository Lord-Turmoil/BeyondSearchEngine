using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class OpenAlexDto
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "created")]
    public DateTime Created { get; set; }

    [JsonProperty(PropertyName = "updated")]
    public DateTime Updated { get; set; }
}