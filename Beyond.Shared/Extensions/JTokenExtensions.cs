using Beyond.Shared.Indexer;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Extensions;

static class JTokenExtensions
{
    public static JToken NotNull(this JToken? token)
    {
        return token ?? throw new IndexException("Unexpected null");
    }

    public static JObject NotNull(this JObject? jObject)
    {
        return jObject ?? throw new IndexException("Unexpected null");
    }

    public static JArray NotNull(this JArray? array)
    {
        return array ?? throw new IndexException("Unexpected null");
    }

    public static string ToStringNotNull(this JToken? token)
    {
        return token.NotNull().ToObject<string>().NotNull();
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

    public static JObject ToJObjectNotNull(this JToken? token)
    {
        return token.NotNull().ToObject<JObject>().NotNull();
    }

    public static JArray ToJArrayNotNull(this JToken? token)
    {
        return token.NotNull().ToObject<JArray>().NotNull();
    }
}