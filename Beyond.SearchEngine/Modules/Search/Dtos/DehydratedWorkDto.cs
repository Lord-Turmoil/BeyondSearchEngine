namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Used for search preview.
/// </summary>
public class DehydratedWorkDto
{
    public string Id { get; set; }
    public string Title { get; set; }

    public int CitationCount { get; set; }
    public int PublicationYear { get; set; }
}