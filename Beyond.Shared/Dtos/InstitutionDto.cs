﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

public class InstitutionDto : OpenAlexStatisticsDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /***             Basics               ***/

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    [JsonProperty(PropertyName = "homepage_url")]
    public string HomepageUrl { get; set; }

    [JsonProperty(PropertyName = "image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty(PropertyName = "thumbnail_url")]
    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));

    [JsonProperty(PropertyName = "associated_institutions")]
    public List<AssociatedInstitutionData> AssociatedInstitutionList { get; set; }

    [JsonIgnore]
    public string AssociatedInstitutions => string.Join(";", AssociatedInstitutionList.Select(i => i.ToString()));
}