using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Deprecated.Dtos;

public class DeprecatedAuthorDto : OpenAlexStatisticsDto
{
    [JsonProperty(PropertyName = "orcid")]
    public string OrcId { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /***                Relation               ***/

    [JsonProperty(PropertyName = "institution")]
    public string Institution => InstitutionData == null ? string.Empty : InstitutionData.Name;

    [JsonIgnore]
    public InstitutionData? InstitutionData { get; set; }

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));
}