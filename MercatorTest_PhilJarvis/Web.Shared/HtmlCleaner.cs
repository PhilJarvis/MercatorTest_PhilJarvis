using HtmlAgilityPack;
using NLog;
using System;
using System.Xml.Linq;

namespace MercatorTest_PhilJarvis.Web.Shared
{
    public sealed class HtmlCleaner
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public XElement CleanAndParse(string html)
        {
            return ParseXml(GetXml(html));
        }

        public XElement ParseXml(string xml)
        {
            try
            {
                return XElement.Parse(xml);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to parse xml");
            }

            return null;
        }

        public string GetXml(string html)
        {
            try
            {
                var doc = new HtmlDocument()
                {
                    OptionFixNestedTags = true,
                    OptionOutputAsXml = true
                };
                doc.LoadHtml(html);
                return doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to parse html");
                throw;
            }
        }
    }
}
