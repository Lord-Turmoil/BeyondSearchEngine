// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Update.Models.Elastic;

public class ElasticPublisher : ElasticStatisticsModel
{
    public string Name { get; set; }


    /***            Basics               ***/

    public string Countries { get; set; }

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    public string ParentPublisher { get; set; }

    public string Roles { get; set; }
}