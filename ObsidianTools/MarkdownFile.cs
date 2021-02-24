using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ObsidianTools
{
    public class MarkdownFile
    {
        private List<MarkdownLink> _links;
        private List<MarkdownLink> _linksEmbedded;

        public MarkdownFile(String path)
        {
            try
            {
                Info = new FileInfo(path);
                Content = File.ReadAllText(path, Encoding.UTF8);
                HasContents = !String.IsNullOrEmpty(Content.Trim());
            }
            catch (Exception x)
            {
                LogHelper.LogException($"An error occurred creating a markdown file instance @ '{path}'", x);
            }
        }

        public Boolean HasChanged { get; private set; }

        public FileInfo Info { get; }
        public String Content { get; private set; }
        public Boolean HasContents { get; }

        public List<MarkdownLink> Links
        {
            get
            {
                return _links ??= MarkdownLink.ForContent(Content);
            }
        }
        public List<MarkdownLink> LinksInklEmbedded
        {
            get
            {
                return _linksEmbedded ??= MarkdownLink.ForContent(Content, false);
            }
        }

        public Boolean IsHealthy()
        {
            return Info.Exists && HasContents;
        }

        public void SaveChanges()
        {
            if (!HasChanged)
            {
                return;
            }

            File.WriteAllText(Info.FullName, Content);
        }

        public void SetContent(String content)
        {
            if (String.Equals(Content, content))
            {
                return;
            }

            HasChanged = true;
            Content = content;
        }

        public override String ToString()
        {
            return Info.Name.Replace(Info.Extension, String.Empty);
        }
    }
}
