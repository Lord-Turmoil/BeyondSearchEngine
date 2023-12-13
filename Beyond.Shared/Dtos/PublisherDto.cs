using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class PublisherDto : OpenAlexStatisticsDto
{
    /***            Basics               ***/

    [JsonProperty(PropertyName = "countries")]
    public List<string> CountryList { get; set; }


    /***              Relations                ***/

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonIgnore]
    public string Countries => string.Join(';', CountryList);

    [JsonProperty(PropertyName = "homepage_url")]
    public string HomepageUrl { get; set; }

    [JsonProperty(PropertyName = "image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty(PropertyName = "thumbnail_url")]
    public string ThumbnailUrl { get; set; }

    [JsonProperty(PropertyName = "parent")]
    public PublisherData? ParentPublisherData { get; set; }

    [JsonIgnore]
    public string ParentPublisher => ParentPublisherData?.ToString() ?? string.Empty;

    [JsonProperty(PropertyName = "roles")]
    public List<RoleData> RoleList { get; set; }

    [JsonIgnore]
    public string Roles => string.Join(';', RoleList.Select(c => c.ToString()));
}