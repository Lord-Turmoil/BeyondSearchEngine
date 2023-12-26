// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

public class FunderData
{
    public FunderData()
    {
    }

    public FunderData(string data)
    {
        string[] values = data.Split(',');
        Id = values[0];
        Name = values[1];
        Award = values[2];
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "award")]
    public string Award { get; set; }

    public override string ToString()
    {
        return $"{Id},{Name},{Award}";
    }

    public static FunderData Build(JObject json)
    {
        return new FunderData {
            Id = json["funder"].ToStringNullable().OpenAlexId(),
            Name = json["funder_display_name"].ToStringNullable(),
            Award = json["award_id"].ToStringNullable()
        };
    }

    public static List<FunderData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data) || data.Contains('&'))
        {
            return [];
        }

        return data.Split(';').Select(c => new FunderData(c)).ToList();
    }
}