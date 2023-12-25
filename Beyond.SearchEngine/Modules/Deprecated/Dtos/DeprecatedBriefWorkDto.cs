using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Deprecated.Dtos;

public class DeprecatedBriefWorkDto : DehydratedWorkDto
{
    [JsonProperty(PropertyName = "abstract")]
    public string Abstract;

    [JsonProperty(PropertyName = "authors")]
    public List<string> Authors => AuthorList.Select(author => author.Name).ToList();

    [JsonIgnore]
    public List<AuthorData> AuthorList { get; set; }
}