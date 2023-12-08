using Beyond.Shared.Data;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class AssociatedInstitutionDataBuilder : IDtoBuilder<AssociatedInstitutionData>
{
    public AssociatedInstitutionData? Build(JObject json)
    {
        return new AssociatedInstitutionData {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Name = json["display_name"].ToStringNotNull("display_name"),
            Type = json["type"].ToStringNotNull("type"),
            Relation = json["type"].ToStringNotNull("type")
        };
    }
}