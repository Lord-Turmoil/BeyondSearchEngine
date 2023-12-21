// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Update.Models.Elastic;

public class ElasticWork : ElasticModel
{
    public string Doi { get; set; }

    /***               Basics               ***/

    public string Title { get; set; }

    public string Abstract { get; set; }

    public string Type { get; set; }

    public string Language { get; set; }

    public string SourceUrl { get; set; }

    public string PdfUrl { get; set; }


    /***               Relations              ***/

    public string Source { get; set; }

    public string Concepts { get; set; }

    public string Keywords { get; set; }

    public string RelatedWorks { get; set; }

    public string ReferencedWorks { get; set; }

    public string Authors { get; set; }

    public string Funders { get; set; }

    /***              Statistics               ***/

    public int CitationCount { get; set; }

    public int PublicationYear { get; set; }

    public DateTime PublicationDate { get; set; }
}