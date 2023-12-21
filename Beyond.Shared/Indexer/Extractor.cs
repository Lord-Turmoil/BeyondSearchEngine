// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;

namespace Beyond.Shared.Indexer;

static class Extractor
{
    public static void Extract(string filename, string destination)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("File not found: " + filename);
        }

        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        // Clear the directory.
        var directory = new DirectoryInfo(destination);
        foreach (FileInfo file in directory.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            subDirectory.Delete(true);
        }

        // Extract the file.
        string ext = Path.GetExtension(filename);
        switch (ext)
        {
            case ".rar":
                ExtractRar(filename, destination);
                break;
            case ".zip":
                ExtractZip(filename, destination);
                break;
            case ".gz":
                ExtractGz(filename, destination);
                break;
            default:
                throw new Exception("Unknown file extension: " + ext);
        }
    }

    private static void ExtractZip(string filename, string destination)
    {
        using (ZipArchive archive = ZipArchive.Open(filename))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destination, new ExtractionOptions {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }
    }

    private static void ExtractRar(string filename, string destination)
    {
        using (RarArchive archive = RarArchive.Open(filename))
        {
            foreach (RarArchiveEntry entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destination, new ExtractionOptions {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }
    }

    private static void ExtractGz(string filename, string destination)
    {
        using (GZipArchive archive = GZipArchive.Open(filename))
        {
            foreach (GZipArchiveEntry entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destination, new ExtractionOptions {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }
    }
}