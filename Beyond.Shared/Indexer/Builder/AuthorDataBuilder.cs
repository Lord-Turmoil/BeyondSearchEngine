using Beyond.Shared.Data;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class AuthorDataBuilder : IDtoBuilder<AuthorData>
{
    public AuthorData? Build(JObject json)
    {
        JObject? authorJObject = json["author"].ToJObjectNullable();
        if (authorJObject == null)
        {
            return null;
        }

        return new AuthorData {
            Position = json["author_position"].ToStringNullable("default"),
            Id = authorJObject["id"].ToStringNullable().OpenAlexId(),
            OrcId = json["orcid"].ToStringNullable().OrcId(),
            Name = authorJObject["display_name"].ToStringNotNull("display_name")
        };
    }
}