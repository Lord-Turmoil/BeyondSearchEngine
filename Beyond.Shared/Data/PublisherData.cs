using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

public class PublisherData
{
    public PublisherData()
    {
    }

    public PublisherData(string data)
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

    public static PublisherData? Build(JObject? json)
    {
        if (json == null)
        {
            return null;
        }

        return new PublisherData {
            Id = json["id"].ToStringNotNull("id"),
            Name = json["name"].ToStringNotNull("name")
        };
    }

    public static PublisherData? Build(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return null;
        }

        return new PublisherData(data);
    }

    public static List<PublisherData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return data.Split(';').Select(c => new PublisherData(c)).ToList();
    }
}