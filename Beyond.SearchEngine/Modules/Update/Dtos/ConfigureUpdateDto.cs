using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Dtos;

public class ConfigureUpdateDto : ApiRequestDto
{
    public string Username;
    public string Password;

    public string DataPath;
    public string TempPath;

    public override bool Verify()
    {
        return !string.IsNullOrEmpty(Username) &&
               !string.IsNullOrEmpty(Password) &&
               !string.IsNullOrEmpty(DataPath) &&
               !string.IsNullOrEmpty(TempPath);
    }
}