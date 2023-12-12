using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

/// <summary>
///     Complete author information.
/// </summary>
public class AuthorDto : OpenAlexStatisticsDto
{
    [JsonProperty(PropertyName = "orcid")]
    public string OrcId { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /***                Relation               ***/

    [JsonProperty(PropertyName = "institution")]
    public InstitutionData? InstitutionData { get; set; }

    [JsonIgnore]
    public string Institution => InstitutionData?.ToString() ?? "";

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));
}