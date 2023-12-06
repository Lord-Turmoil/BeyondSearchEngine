using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

static class InstitutionDtoBuilder
{
    public static InstitutionDto Build(JObject json)
    {
        var dto = new InstitutionDto {
            Id = json["id"].ToStringNotNull().OpenAlexId(),
            Name = json["display_name"].ToStringNotNull(),
            Type = json["type"].ToStringNotNull(),
            Country = json["country_code"].ToStringNotNull(),
            HomepageUrl = json["homepage_url"].ToStringNotNull(),
            ImageUrl = json["image_url"].ToStringNotNull(),
            ThumbnailUrl = json["thumbnail_url"].ToStringNotNull(),

            ConceptList = new List<ConceptData>(),
            AssociatedInstitutionList = new List<AssociatedInstitutionData>(),

            WorksCount = json["works_count"].ToIntNotNull(),
            CitationCount = json["cited_by_count"].ToIntNotNull(),

            Created = json["created_date"].ToDateTimeNotNull(),
            Updated = json["updated_date"].ToDateTimeNotNull()
        };

        foreach (JToken token in json["x_concepts"].NotNull().ToJArrayNotNull())
        {
            dto.ConceptList.Add(ConceptDataBuilder.Build(token.NotNull().ToObject<JObject>().NotNull()));
        }

        foreach (JToken token in json["associated_institutions"].ToJArrayNotNull())
        {
            dto.AssociatedInstitutionList.Add(
                AssociatedInstitutionDataBuilder.Build(token.NotNull().ToObject<JObject>().NotNull()));
        }

        return dto;
    }
}