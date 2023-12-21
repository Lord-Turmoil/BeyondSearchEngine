// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Between <see cref="WorkDto" /> and <see cref="DehydratedWorkDto" />.
/// </summary>
public class BriefWorkDto : DehydratedWorkDto
{
    [JsonProperty(PropertyName = "authors")]
    public List<AuthorData> AuthorList { get; set; }
}