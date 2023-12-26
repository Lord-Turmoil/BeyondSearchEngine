// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class InvalidIdListDto : BadRequestDto
{
    public InvalidIdListDto(string message = "Invalid ID list") : base(message)
    {
    }
}