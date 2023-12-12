using System.ComponentModel.DataAnnotations;

namespace Beyond.SearchEngine.Modules.Update.Models;

public class UpdateConfiguration
{
    /// <summary>
    ///     This ID should always be 1.
    /// </summary>
    [Key]
    public int Id { get; set; }

    public string DataPath { get; set; }
    public string TempPath { get; set; }

    public bool IsConfigured => !string.IsNullOrEmpty(DataPath) && !string.IsNullOrEmpty(TempPath);
}