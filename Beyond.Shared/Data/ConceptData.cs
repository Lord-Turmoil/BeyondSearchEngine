using Newtonsoft.Json;

namespace Beyond.Shared.Data;

public class ConceptData
{
    public ConceptData(string data)
    {
        string[] values = data.Split(',');
        Id = values[0];
        Name = values[1];
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    public override string ToString()
    {
        return $"{Id},{Name}";
    }
}