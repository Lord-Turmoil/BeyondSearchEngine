using Beyond.Shared.Data;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public static class CountsByYearDataBuilder
{
    public static CountsByYearData Build(JObject json)
    {
        return new CountsByYearData {
            Year = json["year"].ToIntNotNull("year", 0),
            WorksCount = json["works_count"].ToIntNotNull("works_count", 0),
            CitationCount = json["citation_count"].ToIntNotNull("citation_count", 0)
        };
    }
}
