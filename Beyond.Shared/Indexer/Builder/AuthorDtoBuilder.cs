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

            InstitutionData = null,
            ConceptList = new List<ConceptData>(),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        JObject? lastKnownInstitution = json["last_known_institution"].ToJObjectNullable();
        if (lastKnownInstitution != null)
        {
            dto.InstitutionData = new InstitutionData {
                Id = lastKnownInstitution["id"].ToStringNullable().OpenAlexId(),
                Name = lastKnownInstitution["display_name"].ToStringNullable(),
                Type = lastKnownInstitution["type"].ToStringNullable(),
                Country = lastKnownInstitution["country_code"].ToStringNullable()
            };
        }

        var conceptDataBuilder = new ConceptDataBuilder();
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            ConceptData? data = conceptDataBuilder.Build(token.ToJObjectNotNull());
            if (data != null)
            {
                dto.ConceptList.Add(data);
            }
        }

        return dto;
    }
}