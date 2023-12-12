using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    public static ConceptData Build(JObject json)
    {
        return new ConceptData {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Name = json["display_name"].ToStringNotNull("display_name"),
            Level = json["level"].ToIntNotNull("level"),
            Score = json["score"].ToDoubleNotNull("score")
        };
    }

    public static List<ConceptData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return data.Split(';').Select(c => new ConceptData(c)).ToList();
    }
}