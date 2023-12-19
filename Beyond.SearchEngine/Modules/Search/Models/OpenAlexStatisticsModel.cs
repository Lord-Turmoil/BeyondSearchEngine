using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

/// <summary>
///     Base class for models with statistics.
/// </summary>
public class OpenAlexStatisticsModel : OpenAlexModel
{
    /// <summary>
    ///     This is a mistake. But I forgot to add Name field to
    ///     Work, so I had to add it here. :(
    /// </summary>
    public string Name { get; set; }

    public int WorksCount { get; set; }
    public int CitationCount { get; set; }
    public int HIndex { get; set; }

    public string CountsByYears { get; set; }

    public List<CountsByYearData> CountsByYearList => CountsByYearData.BuildList(CountsByYears);
}