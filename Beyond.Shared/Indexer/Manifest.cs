using Newtonsoft.Json.Linq;

namespace Beyond.Shared.Indexer;

static class Manifest
{
    public static List<ManifestEntry> ReadManifest(string path)
    {
        string manifestFilename = Path.Join(path, "manifest");
        if (!File.Exists(manifestFilename))
        {
            throw new IndexException($"Manifest file not found: {manifestFilename}");
        }

        JObject manifest = JObject.Parse(File.ReadAllText(manifestFilename));

        return (from JObject entry in manifest["entries"] select new ManifestEntry(entry)).ToList();
    }
}