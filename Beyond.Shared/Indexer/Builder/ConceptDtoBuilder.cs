using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class ConceptDtoBuilder : ElasticDtoBuilder<ConceptDto>
{
    public override ConceptDto? Build(JObject json)
    {
        ConceptDto dto = base.BuildNotNull(json);

        dto.WikiDataId = json["wikidata"].ToStringNullable().WikiDataId();
        dto.Name = json["display_name"].ToStringNotNull("display_name");
        dto.Level = json["level"].ToIntNotNull("level");
        dto.Description = json["description"].ToStringNullable();
        dto.ImageUrl = json["image_url"].ToStringNullable();
        dto.ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable();

        dto.RelatedConceptList = [];
        foreach (JToken token in json["related_concepts"].ToJArrayNullable())
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.RelatedConceptList.Add(data);
        }

        dto.WorksCount = json["works_count"].ToIntNotNull("works_count", 0);
        dto.CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count", 0);
        dto.HIndex = json["summary_stats"].NotNull("summary_stats")["h_index"].ToIntNotNull("h_index");

        dto.CountsByYearList = [];
        foreach (JToken token in json["counts_by_year"].ToJArrayNullable())
        {
            var data = CountsByYearData.Build(token.ToJObjectNotNull());
            dto.CountsByYearList.Add(data);
        }

        return dto;
    }
}