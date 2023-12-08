namespace Beyond.SearchEngine.Modules.Update.Services.Utils;

public static class UpdateMutex
{
    private static readonly object _updateMutex = new();
    private static readonly HashSet<string> _updatingType = new();

    public static bool IsUpdating(string type)
    {
        lock (_updateMutex)
        {
            return _updatingType.Contains(type);
        }
    }

    public static bool BeginUpdate(string type)
    {
        lock (_updateMutex)
        {
            if (_updatingType.Contains(type))
            {
                return false;
            }

            _updatingType.Add(type);
            return true;
        }
    }

    public static bool EndUpdate(string type)
    {
        lock (_updateMutex)
        {
            return _updatingType.Remove(type);
        }
    }
}