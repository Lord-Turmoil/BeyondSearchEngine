// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Search.Models;

public class Publisher : OpenAlexStatisticsModel
{
    /***            Basics               ***/

    public string Countries { get; set; }
    public List<string> CountryList => string.IsNullOrEmpty(Countries) ? [] : Countries.Split(';').ToList();

    public string HomepageUrl { get; set; }

    public string ImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }


    /***              Relations                ***/

    public string ParentPublisher { get; set; }

    public PublisherData? ParentPublisherData => PublisherData.Build(ParentPublisher);

    public string Roles { get; set; }

    public List<RoleData> RoleList => RoleData.BuildList(Roles);
}