// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Tonisoft.AspExtensions.Cors;

public class CorsOptions
{
    public const string CorsSection = "CorsOptions";
    public const string CorsPolicyName = "DefaultPolicy";

    public bool Enable { get; set; } = false;
    public bool AllowAny { get; set; } = true;
    public List<string> Origins { get; set; } = new();
}