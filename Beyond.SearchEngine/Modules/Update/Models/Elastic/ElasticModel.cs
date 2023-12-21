// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Update.Models.Elastic;

public class ElasticModel
{
    public string Id { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public DateTime TrackingTime { get; set; } = DateTime.UtcNow;
}