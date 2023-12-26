// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Utils;
using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Deprecated.Dtos;

public class DeprecatedWorkDto : OpenAlexDto
{
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
            FullDoi = DataMock.RandomDoi();
        }

        if (SourceData == null || string.IsNullOrEmpty(SourceData.Name))
        {
            SourceData = DataMock.RandomSourceData();
        }
    }
}