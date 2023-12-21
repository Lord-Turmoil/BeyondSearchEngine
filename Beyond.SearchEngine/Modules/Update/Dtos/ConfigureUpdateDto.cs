// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Dtos;

public class ConfigureUpdateDto : ApiRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }

    public string DataPath { get; set; }
    public string TempPath { get; set; }

    public override bool Verify()
    {
        return !string.IsNullOrEmpty(Username) &&
               !string.IsNullOrEmpty(Password) &&
               !string.IsNullOrEmpty(DataPath) &&
               !string.IsNullOrEmpty(TempPath);
    }
}