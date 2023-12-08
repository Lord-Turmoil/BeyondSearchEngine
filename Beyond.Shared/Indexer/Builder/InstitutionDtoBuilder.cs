using Beyond.Shared.Data;
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
            Type = json["type"].ToStringNotNull("type"),
            Country = json["country_code"].ToStringNotNull("country_code"),
            HomepageUrl = json["homepage_url"].ToStringNullable(),
            ImageUrl = json["image_url"].ToStringNullable(),
            ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable(),

            ConceptList = new List<ConceptData>(),
            AssociatedInstitutionList = new List<AssociatedInstitutionData>(),

            WorksCount = json["works_count"].ToIntNotNull("works_count"),
            CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count"),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        var conceptDataBuilder = new ConceptDataBuilder();
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            dto.ConceptList.Add(conceptDataBuilder.Build(token.ToJObjectNotNull()));
        }

        foreach (JToken token in json["associated_institutions"].ToJArrayNotNull("associated_institutions"))
        {
            dto.AssociatedInstitutionList.Add(AssociatedInstitutionDataBuilder.Build(token.ToJObjectNotNull()));
        }

        return dto;
    }
}