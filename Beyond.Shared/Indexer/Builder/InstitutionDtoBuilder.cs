﻿using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class InstitutionDtoBuilder : IDtoBuilder<InstitutionDto>
{
    public InstitutionDto? Build(JObject json)
    {
        var dto = new InstitutionDto {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Name = json["display_name"].ToStringNotNull("display_name"),
            Type = json["type"].ToStringNullable(),
            Country = json["country_code"].ToStringNullable(),
            HomepageUrl = json["homepage_url"].ToStringNullable(),
            ImageUrl = json["image_url"].ToStringNullable(),
            ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable(),

            ConceptList = new List<ConceptData>(),
            AssociatedInstitutionList = new List<AssociatedInstitutionData>(),

            WorksCount = json["works_count"].ToIntNotNull("works_count", 0),
            CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count", 0),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        var conceptDataBuilder = new ConceptDataBuilder();
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            ConceptData? data = conceptDataBuilder.Build(token.ToJObjectNotNull());
            if (data != null)
            {
                dto.ConceptList.Add(data);
            }
        }

        var associatedInstitutionDataBuilder = new AssociatedInstitutionDataBuilder();
        foreach (JToken token in json["associated_institutions"].ToJArrayNotNull("associated_institutions"))
        {
            AssociatedInstitutionData? data = associatedInstitutionDataBuilder.Build(token.ToJObjectNotNull());
            if (data != null)
            {
                dto.AssociatedInstitutionList.Add(data);
            }
        }

        return dto;
    }
}