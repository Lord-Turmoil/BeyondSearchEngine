// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public interface IDtoBuilder<TDto>
{
    TDto Build(JObject json);
}