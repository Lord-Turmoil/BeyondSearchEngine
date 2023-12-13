﻿namespace Beyond.SearchEngine.Modules.Update.Models.Elastic;

public class ElasticStatisticsModel : ElasticModel
{
    public int WorksCount { get; set; }
    public int CitationCount { get; set; }
    public int HIndex { get; set; }

    public string CountsByYears { get; set; }
}