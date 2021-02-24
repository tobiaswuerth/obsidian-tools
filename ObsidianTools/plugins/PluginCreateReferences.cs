using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ObsidianTools.plugins
{
    public class PluginCreateReferences : Plugin
    {
        public PluginCreateReferences() : base("--create-references", "Create file reference for all unlinked mentions") { }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Creating references...");
            IEnumerable<String> filePaths = GetMarkdownFilePaths(payload.VaultDirectory);
            List<MarkdownFile> files = ReadFiles(filePaths);
            files.Sort((a, b) => -1 * a.Info.Name.Length.CompareTo(b.Info.Name.Length));
            foreach (MarkdownFile file in files)
            {
                FileInfo info = file.Info;
                String word = info.Name.Replace(info.Extension, String.Empty);
                Console.WriteLine($"Processing '{word}'...");
                files.Except(new []{file}).ToList().ForEach(f =>
                {
                    ProcessFile(f, word, word);
                    ProcessFile(f, word, word.ToLower());
                    ProcessFile(f, word, word.ToUpper());
                });
            }

            Console.WriteLine("Updating files...");
            files.ForEach(f => f.SaveChanges());
            Console.WriteLine("Created references");
        }

        private static void ProcessFile(MarkdownFile f, String word, String keyword)
        {
            String[] parts = f.Content.Split(keyword);
            Int32 partCount = parts.Length;
            if (2 > partCount)
            {
                return;
            }

            Int32 partIndex = 0;
            String content = parts[partIndex];
            while (partIndex < partCount - 1)
            {
                if (StringIsPartOfLink(content))
                {
                    content += keyword + parts[++partIndex];
                    continue;
                }

                String link = String.Equals(word, keyword) ? $"[[{word}]]" : $"[[{word}|{keyword}]]";
                content += link + parts[++partIndex];
                Console.WriteLine($"- Created link for '{link}' in {f.Info.Name}");
            }

            f.SetContent(content);
        }

        private static Boolean StringIsPartOfLink(String content)
        {
            // not replace [[a]], and [b g](www.c.de) hijklm
            Boolean startedQuote = false;
            while (0 < content.Length)
            {
                if (content.EndsWith("[["))
                {
                    return true;
                }

                if (content.EndsWith("]]"))
                {
                    return false;
                }

                if (content.EndsWith("["))
                {
                    return true;
                }

                if (content.EndsWith("("))
                {
                    startedQuote = true;
                }

                if (content.EndsWith("]"))
                {
                    return startedQuote;
                }

                content = content[..^1];
            }

            return false;
        }
    }
}
