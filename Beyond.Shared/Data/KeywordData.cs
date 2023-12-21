// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Data;

public class KeywordData
{
    public KeywordData()
    {
    }

    public KeywordData(string data)
    {
        string[] values = data.Split(',');
        Keyword = values[0];
        Score = double.Parse(values[1]);
    }

    [JsonProperty(PropertyName = "keyword")]
    public string Keyword { get; set; }


    [JsonProperty(PropertyName = "score")]
    public double Score { get; set; }

    public override string ToString()
    {
        return $"{Keyword},{Score}";
    }

    public static KeywordData Build(JObject json)
    {
        return new KeywordData {
            Keyword = json["keyword"].ToStringNotNull("keyword"),
            Score = json["score"].ToDoubleNotNull("score")
        };
    }

    public static List<KeywordData> BuildList(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        return data.Split(';').Select(c => new KeywordData(c)).ToList();
    }
}