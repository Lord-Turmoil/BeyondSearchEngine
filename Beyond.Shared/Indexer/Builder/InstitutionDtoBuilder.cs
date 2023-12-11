﻿using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class InstitutionDtoBuilder : ElasticDtoBuilder<InstitutionDto>
{
    public override InstitutionDto BuildNotNull(JObject json)
    {
        InstitutionDto dto = base.BuildNotNull(json);
        dto.Name = json["display_name"].ToStringNotNull("display_name");
        dto.Type = json["type"].ToStringNullable();
        dto.Country = json["country_code"].ToStringNullable();
        dto.HomepageUrl = json["homepage_url"].ToStringNullable();
        dto.ImageUrl = json["image_url"].ToStringNullable();
        dto.ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable();

        dto.ConceptList = [];
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        dto.AssociatedInstitutionList = [];
        foreach (JToken token in json["associated_institutions"].ToJArrayNotNull("associated_institutions"))
        {
            var data = AssociatedInstitutionData.Build(token.ToJObjectNotNull());
            dto.AssociatedInstitutionList.Add(data);
        }

        dto.WorksCount = json["works_count"].ToIntNotNull("works_count", 0);
        dto.CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count", 0);
        dto.CountsByYearList = [];
        foreach (JToken token in json["counts_by_year"].ToJArrayNullable())
        {
            var data = CountsByYearData.Build(token.ToJObjectNotNull());
            dto.CountsByYearList.Add(data);
        }

        return dto;
    }
}