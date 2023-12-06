using Beyond.Shared.Data;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

static class AssociatedInstitutionDataBuilder
{
    public static AssociatedInstitutionData Build(JObject json)
    {
        string id = json["id"].ToStringNotNull();
        return new AssociatedInstitutionData {
            Id = id.Substring(id.LastIndexOf('/') + 1),
            Name = json["display_name"].ToStringNotNull(),
            Type = json["type"].ToStringNotNull(),
            Relation = json["type"].ToStringNotNull()
        };
    }
}