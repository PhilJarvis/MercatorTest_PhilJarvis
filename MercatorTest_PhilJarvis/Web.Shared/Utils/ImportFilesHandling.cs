using NLog;
using System;
using System.IO;

namespace MercatorTest_PhilJarvis.Web.Shared.Utils
{
    public class ImportFilesHandling
    {
        protected static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string RenameFileBeforeImport(string filepath, string code, string cat)
        {
            var filePrefix = code + "_" + cat + "-";
            var files = Directory.GetFiles(filepath, filePrefix + "*.xslx");
            string fileNewName = filePrefix + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xslx";
            string newFilePath = filepath + @"\" + fileNewName;

            try
            {
                if (files.Length == 1)
                {
                    File.Move(files[0], newFilePath);
                }
                else
                {
                    throw new FileNotFoundException("Imort File not found", fileNewName);
                }

            }
            catch (IOException ex)
            {
                Logger.Error("fFile not Found {0}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred {0}", ex.Message);
                throw;
            }

            return newFilePath;
        }
    }
}
