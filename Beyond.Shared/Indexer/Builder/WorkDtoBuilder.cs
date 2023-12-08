using System.Text;
using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Beyond.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer.Builder;

public class WorkDtoBuilder : IDtoBuilder<WorkDto>
{
    public WorkDto? Build(JObject json)
    {
        if (!HasAbstract(json))
        {
            return null;
        }

        BuildPdfUrl(json["primary_location"]?.ToObject<JObject>(), out string sourceUrl, out string pdfUrl);
        string abstractText = BuildAbstract(json["abstract_inverted_index"]?.ToObject<JObject>());
        var dto = new WorkDto {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Doi = json["doi"].ToStringNotNull("doi").Doi(),
            Title = json["title"].ToStringNotNull("title"),
            Abstract = abstractText,
            Type = json["type"].ToStringNotNull("type"),
            Language = json["language"].ToStringNotNull("language"),
            SourceUrl = sourceUrl,
            PdfUrl = pdfUrl,

            ConceptList = new List<ConceptData>(),
            KeywordList = new List<string>(),
            RelatedWorkList = new List<string>(),
            ReferencedWorkList = new List<string>(),
            AuthorList = new List<AuthorData>(),

            CitationCount = json["cited_by_count"].ToIntNotNull(),
            PublicationYear = json["publication_year"].ToIntNotNull(),
            PublicationDate = json["publication_date"].ToDateTimeNotNull(),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        var conceptDataBuilder = new ConceptDataBuilder();
        foreach (JToken token in json["x_concepts"].ToJArrayNotNull("x_concepts"))
        {
            ConceptData? data = conceptDataBuilder.Build(token.ToJObjectNotNull());
            if (data != null)
            {
                dto.ConceptList.Add(data);
            }
        }

        var keywordDataBuilder = new KeywordDataBuilder();
        foreach (JToken token in json["keywords"].ToJArrayNotNull("keywords"))
        {
            string? data = keywordDataBuilder.Build(token.ToJObjectNotNull());
            if (data != null)
            {
                dto.KeywordList.Add(data);
            }
        }

        foreach (JToken token in json["related_works"].ToJArrayNotNull("related_works"))
        {
            dto.RelatedWorkList.Add(token.ToStringNotNull().OpenAlexId());
        }

        foreach (JToken token in json["referenced_works"].ToJArrayNotNull("referenced_works"))
        {
            dto.ReferencedWorkList.Add(token.ToStringNotNull().OpenAlexId());
        }

        return dto;
    }

    private static bool HasAbstract(JObject json)
    {
        return json["abstract_inverted_index"] != null;
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
            foreach (JToken index in prop.ToJArrayNotNull())
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