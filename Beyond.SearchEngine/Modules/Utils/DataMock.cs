using System.Text;
using Beyond.Shared.Data;

namespace Beyond.SearchEngine.Modules.Utils;

public static class DataMock
{
    private static readonly Random random = new();

    private static readonly string[] SourceNames = {
        "Nagoya JALT journal",
        "Perio J (Online)",
        "FoDEx-Studie",
        "Borobudur Engineering Review",
        "ESTEEM Academic Journal",
        "Moldovan Journal of Health Sciences",
        "Kifah Jurnal Pengabdian Masyarakat",
        "Revista Apthapi",
        "Laborem",
        "SilvaWorld",
        "Journal of Philosophy and Ethics",
        "Archives of Oncology and Cancer Therapy"
    };

    public static string RandomDoi()
    {
        //  https://doi.org/10.1080/14729679.2018.1507831
        var builder = new StringBuilder();
        builder.Append("https://doi.org/");
        builder.Append("10.");
        builder.Append(random.Next(1000, 9999));
        builder.Append('/');
        builder.Append(random.Next(10000000, 99999999));
        builder.Append('.');
        builder.Append(random.Next(1980, 2023));
        builder.Append('.');
        builder.Append(random.Next(1000000, 9999999));
        return builder.ToString();
    }

    public static void MendSourceData(SourceData sourceData)
    {
        if (string.IsNullOrEmpty(sourceData.Name))
        {
            sourceData.Name = RandomSourceName();
        }
    }

    public static SourceData RandomSourceData()
    {
        return new SourceData {
            Id = "",
            Name = RandomSourceName(),
            HostId = "",
            HostName = "",
            Type = "institution"
        };
    }

    private static string RandomSourceName()
    {
        return SourceNames[random.Next(0, SourceNames.Length)];
    }
}