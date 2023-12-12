using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;

namespace Beyond.Shared.Indexer.Impl;

public class SourceIndexer : GenericIndexer<SourceDtoBuilder, SourceDto>
{
    protected SourceIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath,
        tempPath, beginDate, endDate)
    {
    }
}