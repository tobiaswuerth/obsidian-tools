using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ObsidianTools.plugins
{
    public class PluginCleanup : Plugin
    {
        public PluginCleanup() : base("--cleanup", "Cleanup dead links") { }

        protected override void Handle(String[] args, IEnumerable<String> eFiles)
        {
            Console.WriteLine("Starting to cleanup...");
            Int32 replacementsTotal = 0;
            foreach (String filePath in eFiles)
            {
                try
                {
                    // get all links
                    String text = File.ReadAllText(filePath, Encoding.UTF8);
                    MatchCollection matches = Regex.Matches(text
                        , "(.?)(\\[\\[[^\\]]+\\]\\])"
                        , RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    Int32 replacements = 0;
                    foreach (Match match in matches)
                    {
                        if (match.Value.StartsWith("!"))
                        {
                            // ignore embedded links
                            continue;
                        }

                        // validate existance
                        String sMatch = match.Groups[2].Value;
                        String matchWithoutBrackets = sMatch.Replace("[[", "").Replace("]]", "");
                        String[] linkParts = matchWithoutBrackets.Split("|");
                        String link = linkParts[0].Trim();
                        String name = 2 > linkParts.Length ? link : linkParts[1].Trim();

                        String targetFile = Path.Join(args[0], $"{link}.md");
                        if (!FileHelper.TryDeleteEmptyFile(targetFile))
                        {
                            continue;
                        }

                        // cleanup text
                        Console.WriteLine(
                            $"Dead link found: link '{targetFile}' for match '{sMatch}' ('{matchWithoutBrackets}')");
                        text = text.Replace(sMatch, name).Trim();
                        replacements++;
                    }

                    if (0 < replacements)
                    {
                        // persist cleaned up text
                        File.WriteAllText(filePath, text, new UTF8Encoding(false));
                        replacementsTotal += replacements;
                    }
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred cleaning up in path '{filePath}'", x);
                }
            }

            Console.WriteLine($"Cleaning up done, removed {replacementsTotal} dead links");
        }
    }
}
