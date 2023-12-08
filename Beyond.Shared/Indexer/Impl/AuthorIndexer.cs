﻿using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;

namespace Beyond.Shared.Indexer.Impl;

public class AuthorIndexer : GenericIndexer<AuthorDtoBuilder, AuthorDto>
{
    public AuthorIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate)
        : base(dataPath, tempPath, beginDate, endDate)
    {
    }
}