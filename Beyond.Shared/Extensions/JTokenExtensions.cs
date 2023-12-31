﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Globalization;
using Beyond.Shared.Indexer;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Extensions;

static class JTokenExtensions
{
    public static JToken NotNull(this JToken? token)
    {
        return token ?? throw new IndexException("Unexpected null");
    }

    public static JToken NotNull(this JToken? token, string key)
    {
        return token ?? throw new IndexException($"Unexpected null of \"{key}\"");
    }

    public static JObject NotNull(this JObject? jObject)
    {
        return jObject ?? throw new IndexException("Unexpected null");
    }

    public static JObject NotNull(this JObject? jObject, string key)
    {
        return jObject ?? throw new IndexException($"Unexpected null of \"{key}\"");
    }

    public static JArray NotNull(this JArray? array)
    {
        return array ?? throw new IndexException("Unexpected null");
    }

    public static JArray NotNull(this JArray? array, string key)
    {
        return array ?? throw new IndexException($"Unexpected null of \"{key}\"");
    }

    public static string ToStringNotNull(this JToken? token)
    {
        return token.NotNull().ToObject<string>().NotNull();
    }

    public static string ToStringNotNull(this JToken? token, string key)
    {
        return token.NotNull(key).ToObject<string>().NotNull(key);
    }

    public static string ToStringNullable(this JToken? token, string defaultValue = "")
    {
        return token?.ToObject<string>() ?? defaultValue;
    }

    public static int ToIntNotNull(this JToken? token)
    {
        try
        {
            return token.NotNull().ToObject<int>();
        }
        catch (Exception e)
        {
            throw new IndexException($"Unexpected int value: {token}", e);
        }
    }

    public static int ToIntNotNull(this JToken? token, string key)
    {
        try
        {
            return token.NotNull(key).ToObject<int>();
        }
        catch (Exception e)
        {
            throw new IndexException($"Unexpected int value of \"{key}\": {token}", e);
        }
    }

    public static int ToIntNotNull(this JToken? token, string key, int defaultValue)
    {
        try
        {
            return token.NotNull(key).ToObject<int>();
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public static double ToDoubleNotNull(this JToken? token, double defaultValue)
    {
        try
        {
            return token.NotNull().ToObject<double>();
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public static double ToDoubleNotNull(this JToken? token, string key)
    {
        try
        {
            return token.NotNull(key).ToObject<double>();
        }
        catch (Exception e)
        {
            throw new IndexException($"Unexpected double value of \"{key}\": {token}", e);
        }
    }

    public static double ToDoubleNotNull(this JToken? token)
    {
        try
        {
            return token.NotNull().ToObject<double>();
        }
        catch (Exception e)
        {
            throw new IndexException($"Unexpected double value: {token}", e);
        }
    }

    public static DateTime ToDateTimeNotNull(this JToken? token)
    {
        return DateTime.Parse(token.ToStringNotNull());
    }

    public static DateTime ToDateTimeNotNull(this JToken? token, string key)
    {
        return DateTime.Parse(token.ToStringNotNull(key));
    }

    public static DateTime ToDateTimeNotNull(this JToken? token, string key, string format)
    {
        return DateTime.ParseExact(token.ToStringNotNull(key), format, CultureInfo.InvariantCulture);
    }

    public static DateTime? ToDateTimeNullable(this JToken? token, string key, string format)
    {
        string value = token.ToStringNullable(key);
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
    }

    public static JObject ToJObjectNotNull(this JToken? token)
    {
        return token.NotNull().ToObject<JObject>().NotNull();
    }

    public static JObject? ToJObjectNullable(this JToken? token)
    {
        return token?.ToObject<JObject>();
    }

    public static JObject ToJObjectNotNull(this JToken? token, string key)
    {
        return token.NotNull(key).ToObject<JObject>().NotNull(key);
    }

    public static JArray ToJArrayNotNull(this JToken? token)
    {
        return token.NotNull().ToObject<JArray>().NotNull();
    }

    public static JArray ToJArrayNotNull(this JToken? token, string key)
    {
        return token.NotNull(key).ToObject<JArray>().NotNull(key);
    }

    public static JArray ToJArrayNullable(this JToken? token)
    {
        return token?.ToObject<JArray>() ?? new JArray();
    }
}