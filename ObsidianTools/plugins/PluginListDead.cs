using System;
using System.Collections.Generic;
using System.IO;

namespace ObsidianTools.plugins
{
    public class PluginListDead : Plugin
    {
        public PluginListDead() : base("--list-dead", "List all dead links") { }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Searching for dead links...");
            IEnumerable<String> filePaths = GetMarkdownFilePaths(payload.VaultDirectory);
            List<MarkdownFile> files = ReadFiles(filePaths);
            String outputPath = $"obsidiantools-output-dead-{DateTime.Now:yyyyMMdd-HHmmss}.md";
            using (StreamWriter writer = File.CreateText(outputPath))
            {
                writer.WriteLine("### Dead Links");
                writer.WriteLine("The following dead links were found:");
                foreach (MarkdownFile file in files)
                {
                    MarkdownLink fileLink = MarkdownLink.ForFile(file);
                    foreach (MarkdownLink link in file.Links)
                    {
                        String filePath = FileHelper.GetAbsoluteFileName(payload.VaultDirectory, link.FileName);
                        if (!File.Exists(filePath))
                        {
                            writer.WriteLine($" - Referenced file NOT FOUND @ {fileLink} -> {link} (`{filePath}`)");
                        }
                        else if (!FileHelper.HasContent(filePath))
                        {
                            writer.WriteLine($" - Referenced file IS EMPTY @ {fileLink} -> {link} (`{filePath}`)");
                        }
                    }
                }
            }

            Console.WriteLine("Searching for dead links done");
            Console.WriteLine($"Creating result file done, you can find it here: {outputPath}");
        }
    }
}
