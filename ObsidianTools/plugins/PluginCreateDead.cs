using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ObsidianTools.plugins
{
    public class PluginCreateDead : Plugin
    {
        public PluginCreateDead() : base("--create-dead", "Create all dead link files") { }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Creating dead links...");
            IEnumerable<String> filePaths = GetMarkdownFilePaths(payload.VaultDirectory);
            List<MarkdownFile> files = ReadFiles(filePaths);
            List<MarkdownLink> newLinks = new List<MarkdownLink>();
            String outputPath = $"obsidiantools-output-created-{DateTime.Now:yyyyMMdd-HHmmss}.md";

            foreach (MarkdownFile file in files)
            {
                MarkdownLink fileLink = MarkdownLink.ForFile(file);
                foreach (MarkdownLink link in file.Links)
                {
                    String filePath = FileHelper.GetAbsoluteFileName(payload.VaultDirectory, link.FileName);
                    if (File.Exists(filePath))
                    {
                        continue;
                    }

                    File.WriteAllText(filePath
                        , "_automatically created by [obsidian-tools](https://github.com/tobiaswuerth/obsidian-tools)_ ==#todo==");
                    MarkdownLink newLink = MarkdownLink.ForFile(link.FileName);
                    newLinks.Add(newLink);
                }
            }

            using (StreamWriter writer = File.CreateText(outputPath))
            {
                writer.WriteLine("Created the following files and tagged them with #todo");
                writer.WriteLine("### Newly Created Files");
                String references = CollectionHelper.CommaSeparatedList(newLinks.Select(n => n.ToString()));
                writer.WriteLine(references);
            }

            Console.WriteLine("Created dead links");
            Console.WriteLine($"Creating result file done, you can find it here: {outputPath}");
            Console.WriteLine(
                "Note: You might have to restart your Obsidian.md client in order to correctly index all new files");
        }
    }
}
