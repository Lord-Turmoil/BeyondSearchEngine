// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Extensions;
using Newtonsoft.Json;
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

    [JsonProperty(PropertyName = "role")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "works_count")]
    public int WorksCount { get; set; }

    public override string ToString()
    {
        return $"{Type},{Id},{WorksCount}";
    }

    public static RoleData Build(JObject json)
    {
        return new RoleData {
            Type = json["role"].ToStringNotNull("role"),
            Id = json["id"].ToStringNullable().OpenAlexId(),
            WorksCount = json["works_count"].ToIntNotNull("works_count", 0)
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