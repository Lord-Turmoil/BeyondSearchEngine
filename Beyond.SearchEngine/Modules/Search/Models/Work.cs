﻿using System.ComponentModel.DataAnnotations.Schema;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Work : OpenAlexModel
{
    /// <summary>
    ///     DOI without leading url. e.g. 10.1016/s0021-9258(19)52451-6
    ///     of https://doi.org/10.1016/s0021-9258(19)52451-6
    /// </summary>
    [Column(TypeName = "varchar(63)")]
    public string Doi { get; set; }

    [NotMapped]
    public string FullDoi => "https://doi.org/" + Doi;


    /***               Basics               ***/

    [Column(TypeName = "varchar(255)")]
    public string Title { get; set; }

    public string Abstract { get; set; }

    [Column(TypeName = "char(15)")]
    public string Type { get; set; }

    [Column(TypeName = "char(8)")]
    public string Language { get; set; }

    /// <summary>
    ///     Source URL.
    /// </summary>
    [Column(TypeName = "varchar(2083)")]
    public string SourceUrl { get; set; }

    /// <summary>
    ///     Original PDF URL.
    /// </summary>
    [Column(TypeName = "varchar(2083)")]
    public string PdfUrl { get; set; }


    /***               Relations              ***/

    public string Source { get; set; }

    [NotMapped]
    public SourceData SourceData => new(Source);

    /// <summary>
    ///     It is the area of the work. <see cref="ConceptData" />
    ///     It is stored as a string of semi-colon separated concepts.
    ///     id,name,level,score;id,name,level,score;...
    ///     C86803240,Computer Science,0,64.3;C90856448,Zoology,1,32.7
    /// </summary>
    public string Concepts { get; set; }

    [NotMapped]
    public List<ConceptData> ConceptList => ConceptData.BuildList(Concepts);

    /// <summary>
    ///     Keywords are stored as a string of comma separated words.
    ///     e.g. keyword,keyword,keyword,...
    /// </summary>
    public string Keywords { get; set; }

    [NotMapped]
    public List<KeywordData> KeywordList => KeywordData.BuildList(Keywords);

    /// <summary>
    ///     It is stored as a string of comma separated works.
    ///     Only their primary ID.
    ///     W8863503,W8863503,W8863503...
    /// </summary>
    public string RelatedWorks { get; set; }

    [NotMapped]
    public List<string> RelatedWorkList => string.IsNullOrEmpty(RelatedWorks) ? [] : RelatedWorks.Split(';').ToList();

    /// <summary>
    ///     The same as <see cref="RelatedWorks" />.
    /// </summary>
    public string ReferencedWorks { get; set; }

    [NotMapped]
    public List<string> ReferencedWorkList =>
        string.IsNullOrEmpty(ReferencedWorks) ? [] : ReferencedWorks.Split(';').ToList();

    /// <summary>
    ///     It is stored as a string of semi-colon separated authors.
    ///     Each author is represented in a quadruple separated by comma.
    ///     position,OpenAlexId,OrcId,name
    ///     All authors are separated by semi-colon.
    ///     author;author;author;...
    /// </summary>
    public string Authors { get; set; }

    [NotMapped]
    public List<AuthorData> AuthorList => AuthorData.BuildList(Authors);


    /***              Statistics               ***/

    /// <summary>
    ///     Total number of citations.
    /// </summary>
    public int CitationCount { get; set; }

    public int PublicationYear { get; set; }

    public DateTime PublicationDate { get; set; }
}