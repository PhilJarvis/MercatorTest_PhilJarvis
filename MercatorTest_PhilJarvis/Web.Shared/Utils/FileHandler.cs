using System;
using System.IO;
using System.Linq;

namespace MercatorTest_PhilJarvis.Web.Shared.Utils
{
    class FileHandler
    {
        public void DeleteFiles(string directory, string partFilename)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            FileInfo[] files = di.GetFiles(partFilename + "*").Where(p => p.Extension == ".xslx").ToArray();

            foreach (FileInfo file in files)
            {
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The File couldnt be downloaded");
                }
            }
        }
    }
}
