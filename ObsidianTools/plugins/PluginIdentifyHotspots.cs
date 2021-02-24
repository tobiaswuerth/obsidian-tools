using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ObsidianTools.plugins
{
    public class PluginIdentifyHotspots : Plugin
    {
        public PluginIdentifyHotspots() : base("--identify-hotspots", "Find the top nodes which are most often linked to") { }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Starting to identify-hotspots...");
            List<String> markdownFiles = GetMarkdownFilePaths(payload.VaultDirectory).ToList();
            List<MarkdownFile> files = ReadFiles(markdownFiles);
            Dictionary<String, HashSet<String>> referenceCount = CreateReferenceDict(files);

            Console.WriteLine($"Found the following reference counds:");
            List<KeyValuePair<String, HashSet<String>>> orderedResult = referenceCount.OrderByDescending(kv => kv.Value.Count).ToList();
            
            String outputPath = $"obsidiantools-output-hotspots-{DateTime.Now:yyyyMMdd-HHmmss}.md";
            using (StreamWriter writer = File.CreateText(outputPath))
            {
                writer.WriteLine("### Identified Hotspots");
                writer.WriteLine("The following were counted:");
                foreach ((String source, HashSet<String> linkedTo) in orderedResult.Take(10))
                {
                    writer.WriteLine($" - {linkedTo.Count}x [[{source}]]");
                    Console.WriteLine($" - {linkedTo.Count}x {source}");
                }
            }
            
            Console.WriteLine("Identify-hotspots done");
            Console.WriteLine($"Creating result file done, you can find it here: {outputPath}");
        }

        public static Dictionary<String, HashSet<String>> CreateReferenceDict(List<MarkdownFile> files)
        {
            Dictionary<String, HashSet<String>> referenceCount = new Dictionary<String, HashSet<String>>();
            foreach (MarkdownFile file in files)
            {
                try
                {
                    if (!file.IsHealthy() && FileHelper.TryDeleteEmptyFile(file.Info.FullName))
                    {
                        continue;
                    }

                    foreach (MarkdownLink link in file.Links)
                    {
                        if (referenceCount.ContainsKey(link.FileName))
                        {
                            referenceCount[link.FileName].Add(file.Info.Name);
                        }
                        else
                        {
                            referenceCount[link.FileName] = new HashSet<String>
                            {
                                file.Info.Name
                            };
                        }
                    }
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred cleaning up in path '{file.Info.FullName}'", x);
                }
            }

            return referenceCount;
        }
    }
}
