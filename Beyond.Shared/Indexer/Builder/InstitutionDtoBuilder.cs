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
            Type = json["type"].ToStringNullable(),
            Country = json["country_code"].ToStringNullable(),
            HomepageUrl = json["homepage_url"].ToStringNullable(),
            ImageUrl = json["image_url"].ToStringNullable(),
            ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable(),

            ConceptList = new List<ConceptData>(),
            AssociatedInstitutionList = new List<AssociatedInstitutionData>(),

            WorksCount = json["works_count"].ToIntNotNull("works_count", 0),
            CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count", 0),
            CountsByYearList = new List<CountsByYearData>(),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            ConceptData data = ConceptDataBuilder.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        foreach (JToken token in json["associated_institutions"].ToJArrayNotNull("associated_institutions"))
        {
            AssociatedInstitutionData data = AssociatedInstitutionDataBuilder.Build(token.ToJObjectNotNull());
            dto.AssociatedInstitutionList.Add(data);
        }

        foreach (JToken token in json["counts_by_year"].ToJArrayNullable())
        {
            CountsByYearData data = CountsByYearDataBuilder.Build(token.ToJObjectNotNull());
            dto.CountsByYearList.Add(data);
        }

        return dto;
    }
}