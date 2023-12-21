// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class AuthorDtoBuilder : OpenAlexStatisticsDtoBuilder<AuthorDto>
{
    public override AuthorDto Build(JObject json)
    {
        AuthorDto dto = base.Build(json);

        dto.OrcId = json["orcid"].ToStringNullable().OrcId();
        dto.Name = json["display_name"].ToStringNotNull("name");

        dto.InstitutionData = InstitutionData.Build(json["last_known_institution"].ToJObjectNullable());

        dto.ConceptList = [];
        foreach (JToken token in json["x_concepts"].ToJArrayNullable())
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        return dto;
    }
}