using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Wykopowo.Contracts;

namespace Wykopowo.Implementations
{
    public class WykopTitlesExtractor : ITitlesExtractor
    {
        const string ArticlesSelector = "#itemsStream h2 a";

        public List<string> ExtractTitles(string htmlInput)
        {
            var result = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);
            var articles = doc.DocumentNode.QuerySelectorAll(ArticlesSelector);
            foreach (var article in articles)
            {
                if (article != null)
                {
                    var trimmedText = article.InnerText.Trim();
                    trimmedText = trimmedText.Replace("&quot;", "\"");
                    result.Add(trimmedText);
                }
            }

            return result;
        }

        public List<NewsLine> ExtractNews(string htmlInput)
        {
            var result = new List<NewsLine>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);
            var articleAnchors = doc.DocumentNode.QuerySelectorAll(ArticlesSelector);
            foreach (var anchorNode in articleAnchors)
            {
                string id = null;
                string time = null;
                var title = anchorNode.InnerText;
                var parentDiv = anchorNode.ParentNode.ParentNode;
                var root = parentDiv.ParentNode;
                var links = parentDiv.QuerySelectorAll("a.affect[href]");
                var alllinks = links.ToList();
                if (alllinks.Count <= 3)
                {
                    continue;
                }

                alllinks.RemoveAt(2);
                alllinks.RemoveAt(0);
                alllinks.RemoveAt(alllinks.Count - 1);
                alllinks.RemoveAll(r => r.HasClass("unhide"));
                var url = alllinks[0].Attributes.Single(r => r.Name == "href").Value;
                var tags = alllinks.Skip(1).Select(r => r.InnerText).ToList();
                string imgUrl = null;
                if (root != null)
                {
                    var img = root.QuerySelector("img");
                    if (img != null)
                    {
                        var urlAttribute =  img.Attributes.SingleOrDefault(r => r.Value.Contains("http") && r.Value.Contains("://"));
                        if (urlAttribute != null)
                        {
                            imgUrl = urlAttribute.Value;
                        }
                    }

                    var idAttr = root.Attributes.SingleOrDefault(r => r.Name == "data-id");
                    if (idAttr != null)
                    {
                        id = idAttr.Value;
                    }

                    var timeNode = root.QuerySelector("time");
                    if (timeNode != null)
                    {
                        var dateTimeProp = timeNode.Attributes.SingleOrDefault(r => r.Name == "datetime");
                        if (dateTimeProp != null)
                        {
                            time = dateTimeProp.Value;
                        }
                    }
                }

                var newsLine = new NewsLine(title, url, tags, imgUrl);
                newsLine.AddId(id);
                newsLine.AddTime(time);
                result.Add(newsLine);
            }

            return result;
        }
    }
}