// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Modules.Update.Services.Updater;

/// <summary>
///     Used to prevent multiple updates of the same type.
/// </summary>
public static class UpdateMutex
{
    private static readonly object _mutex = new();
    private static readonly HashSet<string> _updatingType = new();

    public static bool IsUpdating(string type)
    {
        lock (_mutex)
        {
            return _updatingType.Contains(type);
        }
    }

    public static bool BeginUpdate(string type)
    {
        lock (_mutex)
        {
            return _updatingType.Add(type);
        }
    }

    public static bool EndUpdate(string type)
    {
        lock (_mutex)
        {
            return _updatingType.Remove(type);
        }
    }
}