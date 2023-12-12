using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

/// <summary>
///     Brief author information, used in relation.
/// </summary>
public class AuthorData
{
    public AuthorData()
    {
    }

    public AuthorData(string data)
    {
        string[] values = data.Split(',');
        Position = values[0];
        Id = values[1];
        OrcId = values[2];
        Name = values[3];
    }

    [JsonProperty(PropertyName = "position")]
    public string Position { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "orcid")]
    public string OrcId { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Position},{Id},{OrcId},{Name}";
    }

    public static AuthorData? Build(JObject json)
    {
        JObject? authorJObject = json["author"].ToJObjectNullable();
        if (authorJObject == null)
        {
            return null;
        }

        return new AuthorData {
            Position = json["author_position"].ToStringNullable("default"),
            Id = authorJObject["id"].ToStringNullable().OpenAlexId(),
            OrcId = json["orcid"].ToStringNullable().OrcId(),
            Name = authorJObject["display_name"].ToStringNotNull("display_name")
        };
    }

    public static List<AuthorData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return data.Split(';').Select(c => new AuthorData(c)).ToList();
    }
}