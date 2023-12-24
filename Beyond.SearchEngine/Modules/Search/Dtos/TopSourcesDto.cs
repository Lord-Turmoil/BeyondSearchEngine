// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class TopSourcesDto
{
    public DehydratedStatisticsModelDto Source { get; set; }
    public double Percent { get; set; }
}