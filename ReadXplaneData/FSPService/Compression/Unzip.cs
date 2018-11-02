using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPService.Compression
{
    public class Unzip
    {
        public static List<String> unzipFile(String filename)
        {
            String ZipPath = filename;
            String extractPath = Path.GetDirectoryName(filename);
            ZipFile.ExtractToDirectory(ZipPath, extractPath);
            return Directory.EnumerateFiles(extractPath).ToList();
        }
    }
}
