using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;

namespace Beyond.Shared.Indexer.Impl;

public class WorkIndexer : GenericIndexer<WorkDtoBuilder, WorkDto>
{
    protected WorkIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath, tempPath, beginDate, endDate)
    {
    }
}