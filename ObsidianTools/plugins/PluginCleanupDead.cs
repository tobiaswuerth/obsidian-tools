using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ObsidianTools.plugins
{
    public class PluginCleanup : Plugin
    {
        public PluginCleanup() : base("--cleanup", "Cleanup dead links") { }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Starting to cleanup...");
            List<String> filePaths = GetMarkdownFilePaths(payload.VaultDirectory).ToList();
            List<MarkdownFile> files = ReadFiles(filePaths);
            Int32 replacementsTotal = 0;
            foreach (MarkdownFile file in files)
            {
                try
                {
                    if (!file.IsHealthy() && FileHelper.TryDeleteEmptyFile(file.Info.FullName))
                    {
                        continue;
                    }

                    // get all links
                    String text = file.Content;
                    Int32 replacements = 0;
                    foreach (MarkdownLink link in file.Links)
                    {
                        // validate existance
                        String filePath = FileHelper.GetAbsoluteFileName(payload.VaultDirectory, link.FileName);
                        if (!FileHelper.TryDeleteEmptyFile(filePath))
                        {
                            continue;
                        }

                        // cleanup text
                        Console.WriteLine($"Dead link found: link '{filePath}' for match '{link}'");
                        text = text.Replace(link.ToString(), link.GetCleanedDisplayName()).Trim();
                        replacements++;
                    }

                    if (0 < replacements)
                    {
                        // persist cleaned up text
                        File.WriteAllText(file.Info.FullName, text, new UTF8Encoding(false));
                        replacementsTotal += replacements;
                    }
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred cleaning up in path '{file.Info.FullName}'", x);
                }
            }

            Console.WriteLine($"Cleaning up done, removed {replacementsTotal} dead links");
        }
    }
}
