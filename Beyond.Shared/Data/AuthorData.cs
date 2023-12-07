using Newtonsoft.Json;

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
}