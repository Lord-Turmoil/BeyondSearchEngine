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
            HIndex = json["summary_stats"].ToJObjectNotNull()["h_index"].ToIntNotNull("h_index"),

            InstitutionData = null,
            ConceptList = new List<ConceptData>(),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        JToken? lastKnownInstitution = json["last_known_institution"];
        if (lastKnownInstitution != null)
        {
            dto.InstitutionData = new InstitutionData {
                Id = lastKnownInstitution["id"].ToStringNotNull("last_known_institution.id").OpenAlexId(),
                Name = lastKnownInstitution["display_name"].ToStringNotNull("last_known_institution.name"),
                Type = lastKnownInstitution["type"].ToStringNotNull("last_known_institution.type"),
                Country = lastKnownInstitution["country_code"].ToStringNotNull("last_known_institution.country_code")
            };
        }

        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            dto.ConceptList.Add(ConceptDataBuilder.Build(token.ToJObjectNotNull()));
        }

        return dto;
    }
}