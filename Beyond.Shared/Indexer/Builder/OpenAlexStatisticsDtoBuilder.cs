using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class OpenAlexStatisticsDtoBuilder<TDto> : OpenAlexDtoBuilder<TDto> where TDto : OpenAlexStatisticsDto, new()
{
    public override TDto Build(JObject json)
    {
        TDto dto = base.Build(json);

        dto.WorksCount = json["works_count"].ToIntNotNull("works_count");
        dto.CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count");
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