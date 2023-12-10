using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class AuthorDtoBuilder : IDtoBuilder<AuthorDto>
{
    public AuthorDto? Build(JObject json)
    {
        var dto = new AuthorDto {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            OrcId = json["orcid"].ToStringNullable().OrcId(),
            Name = json["display_name"].ToStringNotNull("name"),

            WorksCount = json["works_count"].ToIntNotNull("works_count"),
            CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count"),
            HIndex = json["summary_stats"].NotNull()["h_index"].ToIntNotNull("h_index"),
            CountsByYearList = new List<CountsByYearData>(),

            InstitutionData = null,
            ConceptList = new List<ConceptData>(),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        JObject? lastKnownInstitution = json["last_known_institution"].ToJObjectNullable();
        if (lastKnownInstitution != null)
        {
            dto.InstitutionData = InstitutionData.Build(lastKnownInstitution);
        }

        foreach (JToken token in json["x_concepts"].ToJArrayNullable())
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        foreach (JToken token in json["counts_by_year"].ToJArrayNullable())
        {
            var data = CountsByYearData.Build(token.ToJObjectNotNull());
            dto.CountsByYearList.Add(data);
        }

        return dto;
    }
}