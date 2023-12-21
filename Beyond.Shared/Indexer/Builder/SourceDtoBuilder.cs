// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class SourceDtoBuilder : OpenAlexStatisticsDtoBuilder<SourceDto>
{
    public override SourceDto Build(JObject json)
    {
        SourceDto dto = base.Build(json);

        dto.Name = json["display_name"].ToStringNotNull("display_name");
        dto.Type = json["type"].ToStringNullable();
        dto.Country = json["country_code"].ToStringNullable();
        dto.HomepageUrl = json["homepage_url"].ToStringNullable();

        dto.HostId = json["host_organization"].ToStringNullable().OpenAlexId();
        dto.HostName = json["host_organization_name"].ToStringNullable();

        dto.ConceptList = [];
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        return dto;
    }
}