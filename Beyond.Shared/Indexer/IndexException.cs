// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.Shared.Indexer;

class IndexException : Exception
{
    public IndexException(string message) : base(message)
    {
    }

    public IndexException(string message, Exception innerException) : base(message, innerException)
    {
    }
}