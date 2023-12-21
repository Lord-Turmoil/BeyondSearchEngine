// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class SearchFailedDto : InternalServerErrorDto
{
    public SearchFailedDto(string message = "Search failed") : base(message)
    {
    }
}