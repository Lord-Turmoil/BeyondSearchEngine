namespace Beyond.SearchEngine.Modules.Search.Models;

/// <summary>
///     Base class for all models that are used to communicate with ElasticSearch.
///     timestamp is used to store the time of the last update of the model.
///     It is required for Logstash to work properly.
/// </summary>
public class OpenAlexModel
{
    /// <summary>
    ///     OpenAlex ID without prefix. e.g. A5040654425
    ///     of https://openalex.org/A5040654425
    /// </summary>
    public string Id { get; set; }

    public string FullId => "https://openalex.org/" + Id;

    public string Name { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    /// <summary>
    ///     The time of the last update of the model, which is managed
    ///     by MySQL automatically. Should not be mapped to DTO.
    /// </summary>
    public DateTime TrackingTime { get; set; } = DateTime.UtcNow;
}