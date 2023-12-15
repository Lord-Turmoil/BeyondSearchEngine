namespace Beyond.SearchEngine.Extensions.Update;

public class UpdateOptions
{
    public const string UpdateSection = "UpdateOptions";

    public int BulkUpdateSize { get; set; }
    public int ConcurrentUpdate { get; set; }
    public string DataPath { get; set; }
    public string TempPath { get; set; }

}