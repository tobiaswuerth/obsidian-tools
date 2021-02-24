using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ObsidianTools
{
    public class MarkdownLink
    {
        private const String REGEX_MARKDOWN_LINK_PATTERN =
            @"(.?)\[\[([^\#|\]\n\r\t]+)(\#([^\#|\]\n\r\t]+))?(\|([^\#|\]\n\r\t]+))?\]\]";

        private MarkdownLink() { }

        public String FileName { get; set; }
        public String ChapterName { get; set; }
        public String DisplayName { get; set; }

        public static List<MarkdownLink> ForContent(String content, Boolean ignoreEmbedded = true)
        {
            List<MarkdownLink> links = new List<MarkdownLink>();

            MatchCollection matches = Regex.Matches(content, REGEX_MARKDOWN_LINK_PATTERN, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (ignoreEmbedded && match.Value.StartsWith("!"))
                {
                    // ignore embedded links
                    continue;
                }

                links.Add(new MarkdownLink
                {
                    FileName = match.Groups[2].Value.Trim()
                    , ChapterName = match.Groups[4].Value.Trim()
                    , DisplayName = match.Groups[6].Value.Trim()
                });
            }

            return links;
        }

        public static MarkdownLink ForFile(String name)
        {
            return new MarkdownLink
            {
                FileName = name.Trim()
            };
        }

        public static MarkdownLink ForFile(MarkdownFile file)
        {
            return ForFile(file.ToString());
        }

        public String GetCleanedDisplayName()
        {
            if (!String.IsNullOrEmpty(DisplayName))
            {
                return DisplayName;
            }

            return String.IsNullOrEmpty(ChapterName) ? FileName : $"{FileName}#{ChapterName}";
        }

        public override String ToString()
        {
            String link = $"[[{FileName}";
            if (!String.IsNullOrEmpty(ChapterName))
            {
                link += $"#{ChapterName}";
            }

            if (!String.IsNullOrEmpty(DisplayName))
            {
                link += $"|{DisplayName}";
            }

            link += "]]";
            return link;
        }
    }
}
