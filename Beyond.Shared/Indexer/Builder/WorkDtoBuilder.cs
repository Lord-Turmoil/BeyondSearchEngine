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
        JObject? abstractJObject = json["abstract_inverted_index"].ToJObjectNullable();
        string abstractText = BuildAbstract(abstractJObject);

        BuildPdfUrl(json["primary_location"].ToJObjectNullable(), out string sourceUrl, out string pdfUrl);

        var dto = new WorkDto {
            Id = json["id"].ToStringNotNull("id").OpenAlexId(),
            Doi = json["doi"].ToStringNullable().Doi(),
            Title = json["title"].ToStringNotNull("title"),
            Abstract = abstractText,
            Type = json["type"].ToStringNullable(),
            Language = json["language"].ToStringNullable("language"),
            SourceUrl = sourceUrl,
            PdfUrl = pdfUrl,

            ConceptList = new List<ConceptData>(),
            KeywordList = new List<KeywordData>(),
            RelatedWorkList = new List<string>(),
            ReferencedWorkList = new List<string>(),
            AuthorList = new List<AuthorData>(),

            CitationCount = json["cited_by_count"].ToIntNotNull("cited_by_count", 0),
            PublicationYear = json["publication_year"].ToIntNotNull("publication_year"),
            PublicationDate = json["publication_date"].ToDateTimeNotNull("publication_date", "yyyy-MM-dd"),

            Created = json["created_date"].ToDateTimeNotNull("created_date"),
            Updated = json["updated_date"].ToDateTimeNotNull("updated_date")
        };

        foreach (JToken token in json["concepts"].ToJArrayNullable())
        {
            var data = ConceptData.Build(token.ToJObjectNotNull());
            dto.ConceptList.Add(data);
        }

        foreach (JToken token in json["keywords"].ToJArrayNullable())
        {
            var data = KeywordData.Build(token.ToJObjectNotNull());
            dto.KeywordList.Add(data);
        }

        foreach (JToken token in json["related_works"].ToJArrayNullable())
        {
            dto.RelatedWorkList.Add(token.ToStringNotNull().OpenAlexId());
        }

        foreach (JToken token in json["referenced_works"].ToJArrayNullable())
        {
            dto.ReferencedWorkList.Add(token.ToStringNotNull().OpenAlexId());
        }

        foreach (JToken token in json["authorships"].ToJArrayNullable())
        {
            var data = AuthorData.Build(token.ToJObjectNotNull());
            if (data != null)
            {
                dto.AuthorList.Add(data);
            }
        }

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