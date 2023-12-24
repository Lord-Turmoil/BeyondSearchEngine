namespace Beyond.SearchEngine.Modules.Statistics.Models;

/// <summary>
///     A patch for OpenAlex data.
/// </summary>
public class WorkStatistics
{
    /// <summary>
    ///     The OpenAlex work id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     How many reader like the work.
    /// </summary>
    public int Likes { get; set; }

    /// <summary>
    ///     How many reader question the work.
    /// </summary>
    public int Questions { get; set; }

    /// <summary>
    ///     How many people view the work.
    /// </summary>
    public int Views { get; set; }
}