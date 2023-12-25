using System.Text;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Deprecated.Dtos;

public class DeprecatedWorkDto : OpenAlexDto
{
    private static readonly Random random = new();

    private static readonly string[] SourceNames = {
        "PubMed",
        "Springer",
        "IEEE",
        "ACM",
        "Elsevier",
        "Wiley",
        "Taylor & Francis",
        "Oxford University Press",
        "Cambridge University Press",
        "SAGE",
        "Nature",
        "Science"
    };

    [JsonProperty(PropertyName = "doi")]
    public string FullDoi { get; set; }


    /***               Basics               ***/

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty(PropertyName = "abstract")]
    public string Abstract { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "language")]
    public string Language { get; set; }

    [JsonProperty(PropertyName = "source_url")]
    public string SourceUrl { get; set; }

    [JsonProperty(PropertyName = "pdf_url")]
    public string PdfUrl { get; set; }


    /***               Relations              ***/

    [JsonProperty(PropertyName = "source")]
    public SourceData? SourceData { get; set; }

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonProperty(PropertyName = "keywords")]
    public List<KeywordData> KeywordList { get; set; }

    [JsonProperty(PropertyName = "related_works")]
    public List<string> RelatedWorkList { get; set; }

    [JsonProperty(PropertyName = "referenced_works")]
    public List<string> ReferencedWorkList { get; set; }

    [JsonProperty(PropertyName = "authors")]
    public List<AuthorData> AuthorList { get; set; }

    [JsonProperty(PropertyName = "funders")]
    public List<FunderData> FunderList { get; set; }

    /***              Statistics               ***/

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "publication_year")]
    public int PublicationYear { get; set; }

    [JsonProperty(PropertyName = "publication_date")]
    public DateTime? PublicationDate { get; set; }

    public void Mock()
    {
        if (string.IsNullOrEmpty(FullDoi))
        {
            FullDoi = RandomDoi();
        }

        if (SourceData == null || string.IsNullOrEmpty(SourceData.Name))
        {
            SourceData = RandomSourceData();
        }
    }

    private static string RandomDoi()
    {
        //  https://doi.org/10.1080/14729679.2018.1507831
        var builder = new StringBuilder();
        builder.Append("https://doi.org/");
        builder.Append("10.");
        builder.Append(random.Next(1000, 9999));
        builder.Append('/');
        builder.Append(random.Next(10000000, 99999999));
        builder.Append('.');
        builder.Append(random.Next(1980, 2023));
        builder.Append('.');
        builder.Append(random.Next(1000000, 9999999));
        return builder.ToString();
    }

    private static SourceData RandomSourceData()
    {
        return new SourceData {
            Id = "",
            Name = RandomSourceName(),
            HostId = "",
            HostName = "",
            Type = "institution"
        };
    }

    private static string RandomSourceName()
    {
        return SourceNames[random.Next(0, SourceNames.Length)];
    }
}