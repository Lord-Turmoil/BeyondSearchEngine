using Beyond.Shared.Data;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

/// <summary>
///     {
///     "id": "https://openalex.org/C121332964",
///     "wikidata": "https://www.wikidata.org/wiki/Q413",
///     "display_name": "Physics",
///     "level": 0,
///     "score": 65.3
///     },
/// </summary>
static class ConceptDataBuilder
{
    public static ConceptData Build(JObject json)
    {
        return new ConceptData {
            Id = json["id"].ToStringNotNull().OpenAlexId(),
            Name = json["display_name"].ToStringNotNull(),
            Level = json["level"].ToIntNotNull(),
            Score = json["score"].ToDoubleNotNull()
        };
    }
}