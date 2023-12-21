// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.Shared.Indexer.Builder;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Impl;

public class GenericIndexer<TDtoBuilder, TDto> : OpenAlexIndexer
    where TDtoBuilder : IDtoBuilder<TDto>, new()
    where TDto : class
{
    protected GenericIndexer(string dataPath, string tempPath, DateOnly beginDate, DateOnly endDate) : base(dataPath,
        tempPath, beginDate, endDate)
    {
        _logger.Info($"Indexing {typeof(TDto).Name}s");
    }

    [Obsolete("This will cause memory and performance issues. Use AllDto() instead")]
    public List<TDto>? NextDataChunk()
    {
        if (NeedNextManifest())
        {
            if (NextManifestEntry() == null)
            {
                _logger.Info("No more manifest entries");
                return null;
            }
        }

        string archivePath = Path.Join(_dataPath, _currentManifestEntry!.RelativePath);
        List<JObject> data = ExtractData(archivePath);
        _currentManifestEntry = null;

        var dtos = new List<TDto>();
        var builder = new TDtoBuilder();
        foreach (JObject json in data)
        {
            try
            {
                TDto dto = builder.Build(json);
                dtos.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.Log($"Failed to build {typeof(TDto).Name}: {ex.Message}");
            }
        }

        return dtos;
    }

    public IEnumerable<TDto> AllDto()
    {
        if (NeedNextManifest())
        {
            if (NextManifestEntry() == null)
            {
                _logger.Info("No more manifest entries");
                yield break;
            }
        }

        string archivePath = Path.Join(_dataPath, _currentManifestEntry!.RelativePath);
        var builder = new TDtoBuilder();
        TDto dto;
        foreach (JObject json in AllData(archivePath))
        {
            try
            {
                dto = builder.Build(json);
            }
            catch (Exception ex)
            {
                _logger.Log($"Failed to build {typeof(TDto).Name}: {ex.Message}");
                continue;
            }

            yield return dto;
        }

        _currentManifestEntry = null;
    }
}