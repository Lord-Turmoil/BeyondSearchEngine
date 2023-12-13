using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Beyond.SearchEngine.Extensions.Elastic;

public class ElasticOptions
{
    public const string ElasticSection = "ElasticOptions";

    public string DefaultConnection { get; set; }
}