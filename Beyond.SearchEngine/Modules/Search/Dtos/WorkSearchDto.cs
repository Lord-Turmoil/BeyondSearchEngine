﻿using System.Globalization;
using System.Text;
using Beyond.SearchEngine.Modules.Utils;
using Beyond.Shared.Data;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class WorkSearchDto
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
    public string Source { get; set; }

    [JsonIgnore]
    public SourceData? SourceData { get; set; }

    [JsonProperty(PropertyName = "concepts")]
    public List<ConceptData> ConceptList { get; set; }

    [JsonProperty(PropertyName = "keywords")]
    public List<KeywordData> KeywordList { get; set; }

    [JsonProperty(PropertyName = "authors")]
    public List<AuthorData> AuthorList { get; set; }


    /***              Statistics               ***/

    [JsonProperty(PropertyName = "citation_count")]
    public int CitationCount { get; set; }

    [JsonProperty(PropertyName = "publication_year")]
    public int PublicationYear { get; set; }

    [JsonProperty(PropertyName = "publication_date")]
    public DateTime? PublicationDate { get; set; }

    public WorkSearchDto Mock()
    {
        if (string.IsNullOrEmpty(FullDoi))
        {
            FullDoi = DataMock.RandomDoi();
        }

        if (SourceData == null)
        {
            SourceData = DataMock.RandomSourceData();
        }
        else
        {
            DataMock.MendSourceData(SourceData);
        }

        StringBuilder builder = new();
        builder.Append(SourceData.Name).Append(", ");
        if (PublicationDate != null)
        {
            builder.Append(PublicationDate?.ToString("MMMM yyyy", CultureInfo.InvariantCulture));
        }
        else
        {
            builder.Append(PublicationYear);
        }
        builder.Append(", ");
        builder.Append(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Type));
        builder.Append(", ");
        builder.Append(FullDoi);

        Source = builder.ToString();

        return this;
    }
}