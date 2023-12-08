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