namespace Beyond.Shared.Indexer;

public class IndexLogger
{
    private string _logPath;

    public IndexLogger(string logPath)
    {
        _logPath = logPath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_logPath, message + "\n");
    }

    public void LogSub(string message)
    {
        Log($"\t{message}");
    }

    public void Info(string message)
    {
        Log($"[INFO] {message}");
    }

    public void Success(string message)
    {
        Log($"[SUCCESS] {message}");
    }

    public void Error(string message)
    {
        Log($"[ERROR] {message}");
    }

    public void Error(string message, Exception e)
    {
        Error($"[ERROR] {message}: {e.Message}");
    }

    public void Error(Exception e)
    {
        Error($"[ERROR] {e.Message}");
    }
}