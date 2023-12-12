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