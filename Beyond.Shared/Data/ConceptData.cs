using Newtonsoft.Json;

namespace Beyond.Shared.Data;

public class ConceptData
{
    public ConceptData()
    {
    }

    public ConceptData(string data)
    {
        string[] values = data.Split(',');
        Id = values[0];
        Name = values[1];
        Level = int.Parse(values[2]);
        Score = double.Parse(values[3]);
    }


    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "level")]
    public int Level { get; set; }

    [JsonProperty(PropertyName = "score")]
    public double Score { get; set; }

    public override string ToString()
    {
        return $"{Id},{Name},{Level},{Score}";
    }
}