using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

public class RoleData
{
    public RoleData()
    {
    }

    public RoleData(string data)
    {
        string[] values = data.Split(',');
        Type = values[0];
        Id = values[1];
        WorksCount = int.Parse(values[2]);
    }

    public string Type;
    public string Id;
    public int WorksCount;

    public override string ToString()
    {
        return $"{Type},{Id},{WorksCount}";
    }

    public static RoleData Build(JObject json)
    {
        return new RoleData {
            Type = json["type"].ToStringNotNull("type"),
            Id = json["id"].ToStringNotNull("id"),
            WorksCount = json["worksCount"].ToIntNotNull("worksCount")
        };
    }

    public static List<RoleData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return data.Split(';').Select(c => new RoleData(c)).ToList();
    }
}