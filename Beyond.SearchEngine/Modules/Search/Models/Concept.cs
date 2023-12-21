// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Concept : OpenAlexStatisticsModel
{
    /// <summary>
    ///     Wikidata ID without prefix. e.g. Q21198
    ///     of https://www.wikidata.org/wiki/Q21198
    /// </summary>
    public string WikiDataId { get; set; }

    public string FullWikiDataId => "https://www.wikidata.org/wiki/" + WikiDataId;


    /***             Basics               ***/

    public int Level { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relation               ***/

    public string RelatedConcepts { get; set; }

    public List<ConceptData> RelatedConceptList => ConceptData.BuildList(RelatedConcepts);
}