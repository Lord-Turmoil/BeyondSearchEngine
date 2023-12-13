using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class ConceptDto : OpenAlexStatisticsDto
{
    [JsonProperty(PropertyName = "wikidata")]
    public string WikiDataId { get; set; }

    /***             Basics               ***/

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "level")]
    public int Level { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty(PropertyName = "thumbnail_url")]
    public string ThumbnailUrl { get; set; }


    /***              Relation               ***/

    [JsonProperty(PropertyName = "related_concepts")]
    public List<ConceptData> RelatedConceptList { get; set; }

    [JsonIgnore]
    public string RelatedConcepts => string.Join(";", RelatedConceptList.Select(c => c.ToString()));
}