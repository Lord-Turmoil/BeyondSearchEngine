using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace BeyondSearchEngine.Modules.Search.Dtos;

/// <summary>
///     Complete author information.
/// </summary>
public class AuthorDto
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "orcid")]
    public string OrcId { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /***              Statistics               ***/

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "h_index")]
    public int HIndex { get; set; }


    /***                Relation               ***/

    [JsonProperty(PropertyName = "institution")]
    public InstitutionData InstitutionData { get; set; }

    [JsonIgnore]
    public string Institution => InstitutionData.ToString();

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));


    /***              Other                   ***/

    [JsonProperty(PropertyName = "created")]
    public DateTime Created { get; set; }

    [JsonProperty(PropertyName = "updated")]
    public DateTime Updated { get; set; }
}