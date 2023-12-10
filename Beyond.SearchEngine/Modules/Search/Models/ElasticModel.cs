using System.ComponentModel.DataAnnotations.Schema;

namespace Beyond.SearchEngine.Modules.Search.Models;

/// <summary>
///     Base class for all models that are used to communicate with ElasticSearch.
///     timestamp is used to store the time of the last update of the model.
///     It is required for Logstash to work properly.
/// </summary>
public class ElasticModel
{
    /// <summary>
    ///     The time of the last update of the model, which is managed
    ///     by MySQL automatically. Should not be mapped to DTO.
    /// </summary>
    [Column(TypeName = "timestamp")]
    public DateTime TrackingTime { get; set; } = DateTime.UtcNow;
}