// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Search.Services.Exceptions;

public class SearchException : Exception
{
    public SearchException(string message) : base(message)
    {
    }

    public SearchException(string message, Exception innerException) : base(message, innerException)
    {
    }
}