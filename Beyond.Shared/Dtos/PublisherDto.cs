using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class PublisherDto : OpenAlexStatisticsDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /***            Basics               ***/

    [JsonProperty(PropertyName = "countries")]
    public List<string> CountryList;

    [JsonIgnore]
    public string Countries => string.Join(';', CountryList);

    [JsonProperty(PropertyName = "homepage_url")]
    public string HomepageUrl { get; set; }

    [JsonProperty(PropertyName = "image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty(PropertyName = "thumbnail_url")]
    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    [JsonProperty(PropertyName = "parent")]
    public PublisherData? ParentPublisherData;

    [JsonIgnore]
    public string ParentPublisher => ParentPublisherData?.ToString() ?? string.Empty;

    [JsonProperty(PropertyName = "roles")]
    public List<RoleData> RoleList;

    [JsonIgnore]
    public string Roles => string.Join(';', RoleList.Select(c => c.ToString()));
}