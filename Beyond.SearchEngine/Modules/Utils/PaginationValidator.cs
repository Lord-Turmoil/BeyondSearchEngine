namespace Beyond.SearchEngine.Modules.Utils;

public static class PaginationValidator
{
    public static bool IsInvalid(int pageSize, int page)
    {
        if (pageSize is < 1 or > Globals.MaxPageSize)
        {
            return true;
        }

        return page < 0 || pageSize * page >= Globals.MaxPagePressure;
    }
}