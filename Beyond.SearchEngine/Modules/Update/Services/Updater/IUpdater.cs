using Beyond.SearchEngine.Modules.Update.Dtos;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater;

/// <summary>
///     Basic update interface.
/// </summary>
public interface IUpdater
{
    Task Update(string type, InitiateUpdateDto dto, string dataPath, string tempPath);
}