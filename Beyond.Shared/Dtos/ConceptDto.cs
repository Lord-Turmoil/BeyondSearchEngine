using Beyond.Shared.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class ConceptDto : ElasticDto
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
    public List<ConceptData> RelatedConceptList;

    [JsonIgnore]
    public string RelatedConcepts => string.Join(";", RelatedConceptList.Select(c => c.ToString()));


    /***              Statistics               ***/

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "h_index")]
    public int HIndex { get; set; }


    [JsonProperty(PropertyName = "counts_by_year")]
    public List<CountsByYearData> CountsByYearList;

    [JsonIgnore]
    public string CountsByYears => string.Join(";", CountsByYearList.Select(c => c.ToString()));
}