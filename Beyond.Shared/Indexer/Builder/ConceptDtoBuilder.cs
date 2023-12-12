using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class ConceptDtoBuilder : OpenAlexStatisticsDtoBuilder<ConceptDto>
{
    public override ConceptDto Build(JObject json)
    {
        ConceptDto dto = base.Build(json);

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

        return dto;
    }
}