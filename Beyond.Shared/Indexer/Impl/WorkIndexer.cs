// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;

namespace Beyond.Shared.Indexer.Impl;

public class WorkIndexer : GenericIndexer<WorkDtoBuilder, WorkDto>
{
    public WorkIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath,
        tempPath, beginDate, endDate)
    {
    }
}