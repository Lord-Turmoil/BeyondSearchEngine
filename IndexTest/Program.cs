using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Impl;
using Newtonsoft.Json;

namespace IndexTest;

class Program
{
    public static void Main(string[] args)
    {
        string dataPath = @"F:\Development\Projects\Web\BeyondScholar\OpenAlex\institutions";
        string tempPath = @"F:\Development\Projects\Web\BeyondScholar\OpenAlex\temp";

        InstitutionIndexer indexer = new(dataPath, tempPath, new DateOnly(2021, 10, 7), new DateOnly(2023, 12, 1));
        List<InstitutionDto>? chunk = indexer.NextDataChunk();
        while (chunk != null)
        {
            foreach (InstitutionDto dto in chunk)
            {
                Console.WriteLine(JsonConvert.SerializeObject(dto));
            }
            chunk = indexer.NextDataChunk();
        }
    }
}