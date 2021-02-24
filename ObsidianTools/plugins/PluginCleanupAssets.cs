using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ObsidianTools.plugins
{
    public class PluginCleanupAssets : Plugin
    {
        public PluginCleanupAssets() : base("--cleanup-assets", "Move all unreferenced [jpg|jpeg|pdf|png] files into subfolder .\\_UNUSED\\") { }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Starting to cleanup-assets...");
            List<String> unusedVaultFiles = GetVaultFilePaths(payload.VaultDirectory).ToList();
            unusedVaultFiles.RemoveAll(p => p.EndsWith(".md"));
            List<String> markdownFiles = GetMarkdownFilePaths(payload.VaultDirectory).ToList();
            List<MarkdownFile> files = ReadFiles(markdownFiles);
            Int32 movementsTotal = 0;
            List<String> relevantFileExtensions = new List<String>
            {
                ".jpg"
                , ".jpeg"
                , ".pdf"
                , ".png"
            };
            unusedVaultFiles.RemoveAll(f =>
            {
                String lowerName = f.ToLower();
                return !relevantFileExtensions.Any(e => lowerName.EndsWith(e));
            });
            foreach (MarkdownFile file in files)
            {
                try
                {
                    if (!file.IsHealthy() && FileHelper.TryDeleteEmptyFile(file.Info.FullName))
                    {
                        continue;
                    }

                    // get all links
                    IEnumerable<String> referencedFiles = file.LinksInklEmbedded.Select(l => FileHelper.GetAbsoluteFileName(payload.VaultDirectory, l.FileName));
                    unusedVaultFiles = unusedVaultFiles.Except(referencedFiles).ToList();
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred cleaning up in path '{file.Info.FullName}'", x);
                }
            }

            String targetDirectory = Path.Join(payload.VaultDirectory, "_UNUSED");
            if (!Directory.Exists(targetDirectory))
            {
                Console.WriteLine("Creating \\_UNUSED target directory...");
                Directory.CreateDirectory(targetDirectory);
                Console.WriteLine("Creating \\_UNUSED target directory done");
            }
            
            Console.WriteLine($"Moving assets ({unusedVaultFiles.Count}x) assets:");

            foreach (String unusedVaultFile in unusedVaultFiles)
            {
                String fileName = Path.GetFileName(unusedVaultFile);
                String newFileName = Path.Join(targetDirectory, fileName);
                Console.WriteLine($" > Moving asset {unusedVaultFiles} -> {newFileName}...");
                File.Move(unusedVaultFile, newFileName);
                Console.WriteLine($" < Moving asset done");
            }

            Console.WriteLine("Cleanup of unreferenced assets done");
        }
    }
}
