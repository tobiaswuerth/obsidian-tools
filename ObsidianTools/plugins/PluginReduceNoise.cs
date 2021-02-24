using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObsidianTools.plugins
{
    public class PluginReduceNoise : Plugin
    {
        public PluginReduceNoise() : base("--reduce-noise"
            , "(beta) Create a tree-like structure starting from a given entry point") { }

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

        private HashSet<String> GetKeywords(String[] args)
        {
            HashSet<String> keywords = new HashSet<String>();
            if (3 > args.Length)
            {
                Console.WriteLine("Provide additional parameters <entry point>");
                return keywords;
            }

            for (Int32 i = 2 ; i < args.Length ; i++)
            {
                String keyword = args[i]?.Trim() ?? String.Empty;
                if (!String.IsNullOrEmpty(keyword))
                {
                    keywords.Add(keyword);
                }
            }

            return keywords;
        }

        protected override void Handle(PluginPayload payload)
        {
            Console.WriteLine("Starting to reduce-noise...");

            HashSet<String> entryPoints = GetKeywords(payload.ConsoleArguments);
            String entryPoint = entryPoints.FirstOrDefault();
            if (null == entryPoint)
            {
                return;
            }

            Console.WriteLine("Reading vault...");
            List<MarkdownFile> markdowns = ReadFiles(GetMarkdownFilePaths(payload.VaultDirectory));
            HashSet<MarkdownFile> workingSet = new HashSet<MarkdownFile>();
            Dictionary<MarkdownFile, HashSet<MarkdownFile>> result = new Dictionary<MarkdownFile, HashSet<MarkdownFile>>();
            MarkdownFile entryNode =
                markdowns.FirstOrDefault(m => m.Info.Name.Replace(m.Info.Extension, "").Equals(entryPoint));
            if (null == entryNode)
            {
                Console.WriteLine("Entry node not found");
                return;
            }
            
            Console.WriteLine("Starting processing...");
            workingSet.Add(entryNode);
            Dictionary<String, HashSet<String>> referenceCount = CreateReferenceDict(markdowns);
            List<KeyValuePair<String, HashSet<String>>> orderedResult =
                referenceCount.OrderByDescending(kv => kv.Value.Count).ToList();
            while (workingSet.Any())
            {
                List<MarkdownFile> nextSet = new List<MarkdownFile>();
                List<MarkdownFile> markdownFiles = workingSet
                    .OrderByDescending(w => orderedResult.First(o => o.Key.Equals(w.FileName)).Value.Count)
                    .ToList();
                foreach (MarkdownFile file in markdownFiles)
                {
                    result[file] = null;
                }

                foreach (MarkdownFile file in markdownFiles)
                {
                    List<MarkdownFile> referencedFiles = file.Links
                        .Select(l => markdowns.FirstOrDefault(m => m.FileName.Equals(l.FileName)))
                        .Where(p => null != p)
                        .ToList();
                    referencedFiles.RemoveAll(result.ContainsKey);
                    referencedFiles.RemoveAll(p => result.Any(r => r.Value?.Contains(p) ?? false));
                    result[file] = new HashSet<MarkdownFile>(referencedFiles);
                    nextSet.AddRange(result[file]);
                }

                workingSet = new HashSet<MarkdownFile>(nextSet);
            }
            
            Console.WriteLine("Update content...");
            foreach ((MarkdownFile file, HashSet<MarkdownFile> links) in result)
            {
                KeyValuePair<MarkdownFile, HashSet<MarkdownFile>> parent = result.FirstOrDefault(r => r.Value.Contains(file));
                if (null != parent.Value)
                {
                    links.Add(parent.Key);
                }

                List<MarkdownLink> linksToRemove = file.Links.ToList();
                HashSet<String> sLinks = links.Select(l => l.FileName).ToHashSet();
                linksToRemove.RemoveAll(p => sLinks.Contains(p.FileName));
                foreach (MarkdownLink link in linksToRemove)
                {
                    file.SetContent(file.Content.Replace(link.ToString(), link.FileName));
                }

                foreach (String s in file.Links.Select(l => l.FileName))
                {
                    sLinks.Remove(s);
                }

                StringBuilder sbAppendix = new StringBuilder($"{Environment.NewLine}{Environment.NewLine}");
                sbAppendix.Append("see also: ");
                sbAppendix.Append(String.Join(", ", sLinks.Select(t => $"[[{t}]]")));

                file.SetContent(file.Content + sbAppendix);
                List<MarkdownFile> externalReferences = markdowns.Except(links)
                    .Where(p => p.Links.Select(l => l.FileName).Contains(file.FileName))
                    .ToList();
                foreach (MarkdownFile toRemove in externalReferences)
                {
                    foreach (MarkdownLink toRemoveLink in toRemove.Links.Where(l => l.FileName.Equals(file.FileName)))
                    {
                        toRemove.SetContent(toRemove.Content.Replace(toRemoveLink.ToString(), toRemoveLink.FileName));
                    }
                }
            }
            
            Console.WriteLine("Apply changes...");
            markdowns.ForEach(p => p.SaveChanges());

            Console.WriteLine("Reduce-noise done");
        }
    }
}
