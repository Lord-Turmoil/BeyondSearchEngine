using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class PublisherDtoBuilder : OpenAlexStatisticsDtoBuilder<PublisherDto>
{
    public override PublisherDto Build(JObject json)
    {
        PublisherDto dto = base.Build(json);

        dto.CountryList = [];
        foreach (JToken token in json["country_codes"].ToJArrayNullable())
        {
            dto.CountryList.Add(token.ToStringNotNull());
        }

        dto.Name = json["display_name"].ToStringNotNull("display_name");
        dto.HomepageUrl = json["homepage_url"].ToStringNullable();
        dto.ImageUrl = json["image_url"].ToStringNullable();
        dto.ThumbnailUrl = json["image_thumbnail_url"].ToStringNullable();

        dto.ParentPublisherData = PublisherData.Build(json["parent_publisher"].ToJObjectNullable());

        dto.RoleList = [];
        foreach (JToken token in json["roles"].ToJArrayNullable())
        {
            var data = RoleData.Build(token.ToJObjectNotNull());
            dto.RoleList.Add(data);
        }

        return dto;
    }
}