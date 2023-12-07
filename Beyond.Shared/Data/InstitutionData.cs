using Newtonsoft.Json;

namespace Beyond.Shared.Data;

public class InstitutionData
{
    public InstitutionData()
    {
    }

    public InstitutionData(string data)
    {
        string[] values = data.Split(',');
        Id = values[0];
        Name = values[1];
        Type = values[2];
        Country = values[3];
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Id},{Name},{Type},{Country}";
    }
}