using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;

namespace Beyond.Shared.Indexer.Impl;

public class FunderIndexer : GenericIndexer<FunderDtoBuilder, FunderDto>
{
    protected FunderIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath, tempPath, beginDate, endDate)
    {
    }
}