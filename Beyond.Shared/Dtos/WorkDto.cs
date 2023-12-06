using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.Shared.Dtos;

class WorkDto
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "doi")]
    public string Doi { get; set; }


    /***               Basics               ***/

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty(PropertyName = "abstract")]
    public string Abstract { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "language")]
    public string Language { get; set; }

    [JsonProperty(PropertyName = "pdf_url")]
    public string PdfUrl { get; set; }


    /***               Relations              ***/

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }
    [JsonIgnore]
    public string Concepts => string.Join(";", ConceptList.Select(c => c.ToString()));

    [JsonProperty(PropertyName = "keywords")]
    public List<string> KeywordList { get; set; }
    [JsonIgnore]
    public string Keywords => string.Join(",", KeywordList);

    [JsonProperty(PropertyName = "related_works")]
    public List<string> RelatedWorkList { get; set; }
    [JsonIgnore]
    public string RelatedWorks => string.Join(",", RelatedWorkList);

    [JsonProperty(PropertyName = "referenced_works")]
    public List<string> ReferencedWorkList { get; set; }
    [JsonIgnore]
    public string ReferencedWorks => string.Join(",", ReferencedWorkList);

    [JsonProperty(PropertyName = "authors")]
    public List<AuthorData> AuthorList { get; set; }
    [JsonIgnore]
    public string Authors => string.Join(";", AuthorList.Select(a => a.ToString()));


    /***              Statistics               ***/

    [JsonProperty(PropertyName = "citation_count")]
    public string CitationCount { get; set; }

    [JsonProperty(PropertyName = "publication_year")]
    public int PublicationYear { get; set; }

    [JsonProperty(PropertyName = "publication_date")]
    public string PublicationDate { get; set; }


    /***              Other                   ***/

    [JsonProperty(PropertyName = "created")]
    public DateTime Created { get; set; }

    [JsonProperty(PropertyName = "updated")]
    public DateTime Updated { get; set; }
}