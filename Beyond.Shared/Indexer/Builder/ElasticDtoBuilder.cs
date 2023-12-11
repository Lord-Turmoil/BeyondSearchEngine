using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class ElasticDtoBuilder<TDto> : IDtoBuilder<TDto> where TDto : ElasticDto, new()
{
    public virtual TDto Build(JObject json)
    {
        TDto dto = new() {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        return dto;
    }
}