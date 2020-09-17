using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ObsidianTools
{
    public class MarkdownFile
    {
        private List<MarkdownLink> _links;

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

        public FileInfo Info { get; }
        public String Content { get; }
        public Boolean HasContents { get; }

        public List<MarkdownLink> Links
        {
            get
            {
                return _links ??= MarkdownLink.ForContent(Content);
            }
        }

        public Boolean IsHealthy()
        {
            return Info.Exists && HasContents;
        }

        public override String ToString()
        {
            return Info.Name.Replace(Info.Extension, String.Empty);
        }
    }
}
