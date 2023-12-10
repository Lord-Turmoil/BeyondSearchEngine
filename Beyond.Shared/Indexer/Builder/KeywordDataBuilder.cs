using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public static class KeywordDataBuilder
{
    public static string Build(JObject json)
    {
        return json["keyword"].ToStringNotNull("keyword");
    }
}