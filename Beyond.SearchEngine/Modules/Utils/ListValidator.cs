namespace Beyond.SearchEngine.Modules.Utils;

public static class ListValidator
{
    public static bool IsInvalidIdList(IReadOnlyCollection<string> idList)
    {
        return idList.Any(string.IsNullOrEmpty);
    }
}