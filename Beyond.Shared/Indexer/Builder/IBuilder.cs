using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public interface IDtoBuilder<TDto>
{
    TDto? Build(JObject json);
}