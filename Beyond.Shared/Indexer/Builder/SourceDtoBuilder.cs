using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class SourceDtoBuilder : ElasticDtoBuilder<SourceDto>
{
    public override SourceDto Build(JObject json)
    {
        SourceDto dto = base.Build(json);

        dto.Name = json["display_name"].ToStringNotNull("display_name");
        dto.Type = json["type"].ToStringNullable();
        dto.Country = json["country_code"].ToStringNullable();
        dto.HomepageUrl = json["homepage_url"].ToStringNullable();

        dto.HostId = json["host_organization"].ToStringNullable().OpenAlexId();
        dto.HostName = json["host_organization_name"].ToStringNullable();

        dto.ConceptList = [];
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
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