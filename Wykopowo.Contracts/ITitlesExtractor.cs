using System.Collections.Generic;

namespace Wykopowo.Contracts
{
    public interface ITitlesExtractor
    {
        List<string> ExtractTitles(string htmlInput);
        List<NewsLine> ExtractNews(string htmlInput);
    }

    public class NewsLine
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImgUrl { get; set; }
        public string VideoUrl { get; set; }
        public List<string> Tags { get; set; }
        public List<KeyValuePair<string, string>> Attributes { get; set; }

        public NewsLine(string title, string url, List<string> tags, string imgUrl)
        {
            Title = title;
            Url = url;
            Tags = tags;
            ImgUrl = imgUrl;
            Attributes = new List<KeyValuePair<string, string>>();
        }

        private void AddAttribute(string name, string value)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                Attributes.Add(new KeyValuePair<string, string>(name, value));
            }
        }
        public void AddId(string id)
        {
            AddAttribute("id",id);
        }

        public void AddTime(string time)
        {
            AddAttribute("time", time);
        }

        public string ToHtml()
        {
            var tags = string.Join(", ", Tags);
            return @$"<a href='{Url}'>{Title}</a> " + tags;
        }
    }
}