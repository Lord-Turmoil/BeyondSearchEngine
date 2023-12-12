using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

public class SourceData
{
    public SourceData()
    {
    }

    public SourceData(string source)
    {
        string[] parts = source.Split(',');
        Id = parts[0];
        Name = parts[1];
        HostId = parts[2];
        HostName = parts[3];
        Type = parts[4];
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    ///     Maybe an Institution ID or Publisher ID.
    /// </summary>
    [JsonProperty(PropertyName = "host_id")]
    public string HostId { get; set; }

    [JsonProperty(PropertyName = "host_name")]
    public string HostName { get; set; }

    /// <summary>
    ///     Type of host.
    /// </summary>
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    public override string ToString()
    {
        return $"{Id},{Name},{HostId},{HostName},{Type}";
    }

    public static SourceData Build(JObject json)
    {
        return new SourceData {
            Id = json["id"].ToStringNullable().OpenAlexId(),
            Name = json["name"].ToStringNullable(),
            HostId = json["host_id"].ToStringNullable().OpenAlexId(),
            HostName = json["host_name"].ToStringNullable(),
            Type = json["type"].ToStringNullable()
        };
    }

    public static List<SourceData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return new List<SourceData>();
        }

        return data.Split(';').Select(c => new SourceData(c)).ToList();
    }
}