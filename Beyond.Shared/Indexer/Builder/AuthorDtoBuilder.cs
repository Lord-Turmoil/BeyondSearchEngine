using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class AuthorDtoBuilder : ElasticDtoBuilder<AuthorDto>
{
    public override AuthorDto Build(JObject json)
    {
        AuthorDto dto = base.Build(json);

        dto.OrcId = json["orcid"].ToStringNullable().OrcId();
        dto.Name = json["display_name"].ToStringNotNull("name");

        dto.WorksCount = json["works_count"].ToIntNotNull("works_count");
        dto.CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count");
        dto.HIndex = json["summary_stats"].NotNull("summary_stats")["h_index"].ToIntNotNull("h_index");
        dto.CountsByYearList = [];
        foreach (JToken token in json["counts_by_year"].ToJArrayNullable())
        {
            var data = CountsByYearData.Build(token.ToJObjectNotNull());
            dto.CountsByYearList.Add(data);
        }

        dto.InstitutionData = null;
        JObject? lastKnownInstitution = json["last_known_institution"].ToJObjectNullable();
        if (lastKnownInstitution != null)
        {
            dto.InstitutionData = InstitutionData.Build(lastKnownInstitution);
        }

        dto.ConceptList = [];
        foreach (JToken token in json["x_concepts"].ToJArrayNullable())
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        return dto;
    }
}