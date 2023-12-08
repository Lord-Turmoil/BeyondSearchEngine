using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Dtos;

public class InitiateUpdateDto : ApiRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }

    /// <summary>
    ///     Absolute path of the data directory.
    /// </summary>
    public string DataPath { get; set; }

    public string TempPath => Path.Join(DataPath, "tmp");

    /// <summary>
    ///     Begin date of the update. Inclusive.
    /// </summary>
    public DateOnly Begin { get; set; }

    /// <summary>
    ///     End date of the update. Inclusive.
    /// </summary>
    public DateOnly End { get; set; }

    public override bool Verify()
    {
        return Begin <= End;
    }
}

public class InitiateUpdateSuccessDto
{
    public DateOnly Begin;
    public DateOnly End;
    public string Type;

    public InitiateUpdateSuccessDto(string type, DateOnly begin, DateOnly end)
    {
        Type = type;
        Begin = begin;
        End = end;
    }
}