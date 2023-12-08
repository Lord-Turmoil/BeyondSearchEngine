using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class KeywordDataBuilder : IDtoBuilder<string>
{
    public string? Build(JObject json)
    {
        return json["keyword"].ToStringNotNull("keyword");
    }
}