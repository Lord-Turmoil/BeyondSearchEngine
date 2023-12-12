using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

public class AssociatedInstitutionData
{
    public AssociatedInstitutionData(string data)
    {
        string[] values = data.Split(',');
        Id = values[0];
        Name = values[1];
        Type = values[2];
        Country = values[3];
        Relation = values[4];
    }

    public AssociatedInstitutionData()
    {
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    [JsonProperty(PropertyName = "relation")]
    public string Relation { get; set; }

    public override string ToString()
    {
        return $"{Id},{Name},{Type},{Country},{Relation}";
    }

    public static AssociatedInstitutionData Build(JObject json)
    {
        return new AssociatedInstitutionData {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Name = json["display_name"].ToStringNotNull("display_name"),
            Type = json["type"].ToStringNotNull("type"),
            Relation = json["type"].ToStringNotNull("type")
        };
    }

    public static List<AssociatedInstitutionData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return data.Split(';').Select(c => new AssociatedInstitutionData(c)).ToList();
    }
}