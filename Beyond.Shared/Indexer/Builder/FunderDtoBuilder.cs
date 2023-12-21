// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class FunderDtoBuilder : OpenAlexStatisticsDtoBuilder<FunderDto>
{
    public override FunderDto Build(JObject json)
    {
        FunderDto dto = base.Build(json);

        dto.Name = json["display_name"].ToStringNotNull("display_name");
        dto.Country = json["country_code"].ToStringNullable();
        dto.Description = json["description"].ToStringNullable();
        dto.HomepageUrl = json["homepage_url"].ToStringNullable();
        dto.ImageUrl = json["image_url"].ToStringNullable();
        dto.ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable();

        dto.RoleList = [];
        foreach (JToken token in json["roles"].ToJArrayNullable())
        {
            var data = RoleData.Build(token.ToJObjectNotNull());
            dto.RoleList.Add(data);
        }

        dto.GrantsCount = json["grants_count"].ToIntNotNull("grants_count", 0);

        return dto;
    }
}