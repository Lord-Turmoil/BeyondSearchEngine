﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Text;
using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class WorkDtoBuilder : OpenAlexDtoBuilder<WorkDto>
{
    public override WorkDto Build(JObject json)
    {
        WorkDto dto = base.Build(json);

        JObject? abstractJObject = json["abstract_inverted_index"].ToJObjectNullable();
        string abstractText = BuildAbstract(abstractJObject);

        JObject? primaryLocation = json["primary_location"].ToJObjectNullable();
        BuildPdfUrl(primaryLocation, out string sourceUrl, out string pdfUrl);

        dto.Doi = json["doi"].ToStringNullable().Doi();
        dto.Title = json["title"].ToStringNullable();
        dto.Abstract = abstractText;
        dto.Type = json["type"].ToStringNullable();
        dto.Language = json["language"].ToStringNullable("language");
        dto.SourceUrl = sourceUrl;
        dto.PdfUrl = pdfUrl;

        dto.SourceData = SourceData.Build(primaryLocation?["source"].ToJObjectNullable());

        dto.ConceptList = [];
        foreach (JToken token in json["concepts"].ToJArrayNullable())
        {
            var data = ConceptData.Build(token.ToJObjectNotNull("concept"));
            dto.ConceptList.Add(data);
        }

        dto.KeywordList = [];
        foreach (JToken token in json["keywords"].ToJArrayNullable())
        {
            var data = KeywordData.Build(token.ToJObjectNotNull("keyword"));
            dto.KeywordList.Add(data);
        }

        dto.RelatedWorkList = [];
        foreach (JToken token in json["related_works"].ToJArrayNullable())
        {
            dto.RelatedWorkList.Add(token.ToStringNullable().OpenAlexId());
        }

        dto.ReferencedWorkList = [];
        foreach (JToken token in json["referenced_works"].ToJArrayNullable())
        {
            dto.ReferencedWorkList.Add(token.ToStringNullable().OpenAlexId());
        }

        dto.AuthorList = [];
        foreach (JToken token in json["authorships"].ToJArrayNullable())
        {
            var data = AuthorData.Build(token.ToJObjectNullable());
            if (data != null)
            {
                dto.AuthorList.Add(data);
            }
        }

        dto.FunderList = [];
        foreach (JToken token in json["funders"].ToJArrayNullable())
        {
            var data = FunderData.Build(token.ToJObjectNotNull("funder"));
            dto.FunderList.Add(data);
        }

        dto.CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count", 0);
        dto.PublicationYear = json["publication_year"].ToIntNotNull("publication_year", 0);
        dto.PublicationDate = json["publication_date"].ToDateTimeNullable("publication_date", "yyyy-MM-dd");

        return dto;
    }

    private static string BuildAbstract(JObject? json)
    {
        if (json == null)
        {
            return string.Empty;
        }

        SortedDictionary<int, string> invertedIndex = new();
        foreach (JProperty prop in json.Properties())
        {
            foreach (JToken index in prop.Value.ToJArrayNotNull())
            {
                invertedIndex.Add((int)index, prop.Name);
            }
        }

        StringBuilder builder = new();
        foreach (KeyValuePair<int, string> entry in invertedIndex)
        {
            if (entry.Key != 0)
            {
                builder.Append(' ');
            }

            builder.Append(entry.Value);
        }

        return builder.ToString();
    }

    private static void BuildPdfUrl(JObject? json, out string srcUrl, out string pdfUrl)
    {
        if (json == null)
        {
            srcUrl = string.Empty;
            pdfUrl = string.Empty;
        }
        else
        {
            srcUrl = json["landing_page_url"].ToStringNullable();
            pdfUrl = json["pdf_url"].ToStringNullable();
        }
    }
}