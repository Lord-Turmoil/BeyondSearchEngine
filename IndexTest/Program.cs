// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Impl;
using Newtonsoft.Json;
using System;

namespace IndexTest;

class Program
{
    public static void Main(string[] args)
    {
        //string dataPath = @"F:\Development\Projects\Web\BeyondScholar\OpenAlex\institutions";
        //string tempPath = @"F:\Development\Projects\Web\BeyondScholar\OpenAlex\temp";

        //InstitutionIndexer indexer = new(dataPath, tempPath, new DateOnly(2021, 10, 7), new DateOnly(2023, 12, 1));
        //List<InstitutionDto>? chunk = indexer.NextDataChunk();
        //while (chunk != null)
        //{
        //    foreach (InstitutionDto dto in chunk)
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(dto));
        //    }
        //    chunk = indexer.NextDataChunk();
        //}

        string url = "s3://openalex/data/institutions/updated_date=2023-10-07/part_013.gz";
        string filename = Path.GetFileNameWithoutExtension(url);
        int id = int.Parse(filename.Substring(filename.Length - 3));
        Console.Out.WriteLine(id);
    }
}