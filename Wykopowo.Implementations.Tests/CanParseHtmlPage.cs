using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Wykopowo.Implementations.Tests
{
    
    public class WykopTitlesExtractorTests
    {
        [Fact]
        public void CanParseHtmlPage()
        {
            var path = GetFullFilePath("Test_data\\page.html");
            var html = File.ReadAllText(path);
            var parser = new WykopTitlesExtractor();
            var headers = parser.ExtractTitles(html);
            Assert.Equal(35, headers.Count);
        }

        [Fact]
        public void CanParseNewsLines()
        {
            var path = GetFullFilePath("Test_data\\page.html");
            var html = File.ReadAllText(path);
            var parser = new WykopTitlesExtractor();
            var titles = parser.ExtractNews(html);
            foreach (var newsLine in titles)
            {
                Assert.NotEmpty(newsLine.Title);
                Assert.NotEmpty(newsLine.ImgUrl);
                Assert.True(newsLine.Tags.Any());
                Assert.NotEmpty(newsLine.Attributes.Single(e=>e.Key=="id").Value);
                var time = newsLine.Attributes.SingleOrDefault(a => a.Key == "time").Value;
                Assert.True(DateTime.TryParse(time, out DateTime dt));
            }
        }

        private string GetFullFilePath(string relPath)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, relPath);
        }
        
    }
}