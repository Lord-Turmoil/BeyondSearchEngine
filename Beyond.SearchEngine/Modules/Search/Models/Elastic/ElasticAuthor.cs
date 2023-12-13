﻿namespace Beyond.SearchEngine.Modules.Search.Models.Elastic;

public class ElasticAuthor : ElasticStatisticsModel
{
    public string OrcId { get; set; }
    public string Name { get; set; }

    /***                Relation               ***/

    public string Institution { get; set; }
    public string Concepts { get; set; }
}