// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Utils;

public static class ListValidator
{
    public static bool IsInvalidIdList(IReadOnlyCollection<string> idList)
    {
        return idList.Any(string.IsNullOrEmpty);
    }
}