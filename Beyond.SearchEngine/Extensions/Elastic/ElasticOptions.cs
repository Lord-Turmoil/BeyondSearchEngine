// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Extensions.Elastic;

public class ElasticOptions
{
    public const string ElasticSection = "ElasticOptions";

    public string DefaultConnection { get; set; }
    public bool EnableBasicAuth { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}