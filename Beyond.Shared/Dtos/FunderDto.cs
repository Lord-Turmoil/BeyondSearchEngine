// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class FunderDto : OpenAlexStatisticsDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /***             Basics               ***/

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "homepage_url")]
    public string HomepageUrl { get; set; }

    [JsonProperty(PropertyName = "image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty(PropertyName = "thumbnail_url")]
    public string ThumbnailUrl { get; set; }


    /***             Relations                ***/

    [JsonProperty(PropertyName = "roles")]
    public List<RoleData> RoleList { get; set; }

    [JsonIgnore]
    public string Roles => string.Join(';', RoleList.Select(c => c.ToString()));


    /***             Statistics                ***/

    [JsonProperty(PropertyName = "grants_count")]
    public int GrantsCount { get; set; }
}