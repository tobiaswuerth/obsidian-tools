using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ObsidianTools.plugins
{
    public class PluginFindReferences : Plugin
    {
        public PluginFindReferences() : base("--find-references", "Find all files referencing the given keywords") { }

        private void CreateResultFile(HashSet<String> keywords, List<MarkdownFile> files)
        {
            Console.WriteLine("Creating result file...");
            String outputPath = $"obsidiantools-output-references-{DateTime.Now:yyyyMMdd-HHmmss}.md";
            using (StreamWriter writer = File.CreateText(outputPath))
            {
                writer.WriteLine("Found the following references");
                writer.WriteLine($"### {CollectionHelper.CommaSeparatedList(keywords)}");
                List<MarkdownLink> links = files.Select(MarkdownLink.ForFile).ToList();
                String referenceList = FileHelper.CreateMarkdownLinkList(links);
                writer.WriteLine(referenceList);
            }

            Console.WriteLine($"Creating result file done, you can find it here: {outputPath}");
        }

        private HashSet<MarkdownFile> FindReferences(IEnumerable<String> eFiles, HashSet<String> keywords)
        {
            Console.WriteLine("Starting to search for word references...");
            HashSet<MarkdownFile> files = new HashSet<MarkdownFile>();
            foreach (String filePath in eFiles)
            {
                try
                {
                    MarkdownFile file = new MarkdownFile(filePath);
                    String lText = file.Content.ToLower();
                    if (keywords.Any(k => lText.Contains(k)))
                    {
                        files.Add(file);
                    }
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred searching for file references in path '{filePath}'", x);
                }
            }

            Console.WriteLine("Searching for references done");
            return files;
        }

        private HashSet<String> GetKeywords(String[] args)
        {
            HashSet<String> keywords = new HashSet<String>();
            if (3 > args.Length)
            {
                Console.WriteLine("Provide additional parameters <keyword1> [<keyword2>, ..]");
                return keywords;
            }

            for (Int32 i = 2 ; i < args.Length ; i++)
            {
                String keyword = args[i]?.Trim().ToLower() ?? String.Empty;
                if (!String.IsNullOrEmpty(keyword))
                {
                    keywords.Add(keyword);
                }
            }

            return keywords;
        }

        protected override void Handle(PluginPayload payload)
        {
            HashSet<String> keywords = GetKeywords(payload.ConsoleArguments);
            if (1 > keywords.Count)
            {
                Console.WriteLine("No valid keyword found");
                return;
            }

            IEnumerable<String> filePaths = GetMarkdownFilePaths(payload.VaultDirectory);
            List<MarkdownFile> files = FindReferences(filePaths, keywords).ToList();
            CreateResultFile(keywords, files);
        }
    }
}
