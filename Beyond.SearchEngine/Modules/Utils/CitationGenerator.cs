// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Text;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Utils;

public static class CitationGenerator
{
    public static string? GenerateCitation(string type, Work work)
    {
        var builder = new StringBuilder();
        switch (type)
        {
            case "bibtex":
                GenerateBibtex(work, builder);
                break;
            case "endnote":
                GenerateEndNote(work, builder);
                break;
            case "acm":
                GenerateAcm(work, builder);
                break;
            default:
                return null;
        }

        return builder.ToString();
    }

    private static void GenerateBibtex(Work work, StringBuilder builder)
    {
        builder.Append('@').Append(GetBibtexEntryType(work.Type)).Append('{');
        builder.Append(string.IsNullOrEmpty(work.Doi) ? work.Id : work.Doi);
        builder.AppendLine(",");
        builder.Append("title = {").Append(work.Title).AppendLine("},");
        builder.Append("year = {").Append(work.PublicationYear).AppendLine("},");
        if (!string.IsNullOrEmpty(work.Doi))
        {
            builder.Append("doi = {").Append(work.Doi).AppendLine("},");
        }

        List<AuthorData> authorList = work.AuthorList;
        if (authorList.Count > 0)
        {
            builder.Append("author = {")
                .Append(string.Join(", ", authorList.Select(x => x.Name)))
                .AppendLine("},");
        }

        SourceData? sourceData = work.SourceData;
        if (sourceData != null)
        {
            builder.Append("publisher = {")
                .Append(sourceData.Name)
                .AppendLine("},");
        }

        List<KeywordData> keywordList = work.KeywordList;
        if (keywordList.Count > 0)
        {
            builder.Append("keywords = {")
                .Append(string.Join(", ", keywordList.Select(x => x.Keyword)))
                .AppendLine("},");
        }

        // if (!string.IsNullOrEmpty(work.Abstract))
        // {
        //     builder.Append("abstract = {")
        //         .Append(work.Abstract)
        //         .AppendLine("},");
        // }

        builder.AppendLine("}");
    }

    private static void GenerateEndNote(Work work, StringBuilder builder)
    {
        builder.Append("%0 ").AppendLine(GetEndNoteEntryType(work.Type));
        builder.Append("%T ").AppendLine(work.Title);
        builder.Append("%D ").AppendLine(work.PublicationYear.ToString());
        if (!string.IsNullOrEmpty(work.Doi))
        {
            builder.Append("%R ").AppendLine(work.FullDoi);
        }

        foreach (AuthorData author in work.AuthorList)
        {
            builder.Append("%A ").AppendLine(author.Name);
        }

        SourceData? sourceData = work.SourceData;
        if (sourceData != null)
        {
            builder.Append("%I ").AppendLine(sourceData.Name);
        }

        List<KeywordData> keywordList = work.KeywordList;
        if (keywordList.Count > 0)
        {
            builder.Append("%K ").AppendLine(string.Join(", ", keywordList.Select(x => x.Keyword)));
        }
    }

    private static void GenerateAcm(Work work, StringBuilder builder)
    {
        bool first = true;
        foreach (AuthorData author in work.AuthorList)
        {
            if (!first)
            {
                builder.Append(", ");
            }
            else
            {
                first = false;
            }

            builder.Append(author.Name);
        }

        if (!first)
        {
            builder.Append(". ");
        }

        builder.Append(work.PublicationYear).Append(". ");

        builder.Append(work.Title).Append('.');

        SourceData? sourceData = work.SourceData;
        if (sourceData != null)
        {
            builder.Append(sourceData.Name).Append(". ");
        }

        if (!string.IsNullOrEmpty(work.Doi))
        {
            builder.Append(work.FullDoi).Append(". ");
        }

        builder.AppendLine();
    }

    private static string GetBibtexEntryType(string openAlexType)
    {
        return openAlexType switch {
            "article" => "article",
            "book-chapter" => "incollection",
            "dissertation" => "phdthesis",
            "book" => "book",
            _ => "misc"
        };
    }

    private static string GetEndNoteEntryType(string openAlexType)
    {
        return openAlexType switch {
            "book" => "Book",
            "book-chapter" => "Book",
            "article" => "Journal Article",
            "dissertation" => "Thesis",
            "report" => "Report",
            _ => "Conference Paper"
        };
    }
}