using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ObsidianTools.plugins
{
    public class PluginFindReferences : Plugin
    {
        public PluginFindReferences() : base("--find-references", "Find all files referencing the given keywords") { }

        private void CreateResultFile(HashSet<String> keywords, HashSet<String> files)
        {
            Console.WriteLine("Creating result file...");
            String outputPath = $"obsidiantools-output-references-{DateTime.Now:yyyyMMdd-HHmmss}.md";
            using (StreamWriter writer = File.CreateText(outputPath))
            {
                writer.WriteLine("Found the following references");
                writer.WriteLine($"### {CollectionHelper.CommaSeparatedList(keywords)}");
                String referenceList = FileHelper.CreateFileReferenceList(files.ToList());
                writer.WriteLine(referenceList);
            }

            Console.WriteLine($"Creating result file done, you can find it here: {outputPath}");
        }

        private HashSet<String> FindReferences(IEnumerable<String> eFiles, HashSet<String> keywords)
        {
            Console.WriteLine("Starting to search for word references...");
            HashSet<String> files = new HashSet<String>();
            foreach (String filePath in eFiles)
            {
                try
                {
                    String text = File.ReadAllText(filePath, Encoding.UTF8);
                    String lText = text.ToLower();
                    if (keywords.Any(k => lText.Contains(k)))
                    {
                        files.Add(FileHelper.GetFileNameWithoutExtension(filePath));
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

        protected override void Handle(String[] args, IEnumerable<String> eFiles)
        {
            HashSet<String> keywords = GetKeywords(args);
            if (1 > keywords.Count)
            {
                Console.WriteLine("No valid keyword found");
                return;
            }

            HashSet<String> files = FindReferences(eFiles, keywords);
            CreateResultFile(keywords, files);
        }
    }
}
