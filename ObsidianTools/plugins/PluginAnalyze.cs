using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ObsidianTools.plugins
{
    public class PluginAnalyze : Plugin
    {
        private readonly Dictionary<String, WordStatistic> _wordStatistics = new Dictionary<String, WordStatistic>();

        public PluginAnalyze() : base("--analyze", "Analyze markdown files and create word report") { }

        private List<WordStatistic> AnalyzeMarkdownFiles(List<MarkdownFile> files)
        {
            Console.WriteLine("Starting to analyze words in files...");
            foreach (MarkdownFile file in files)
            {
                try
                {
                    HandleFile(file);
                }
                catch (Exception x)
                {
                    LogHelper.LogException("An error occurred handling the file", x);
                }
            }

            Console.WriteLine("Analyzing words done");
            List<WordStatistic> stats = _wordStatistics.Values.ToList();
            PrintStats(stats, (a, b) => a.Count.CompareTo(b.Count) * -1);
            return stats;
        }

        private void CreateResultFile(List<WordStatistic> stats)
        {
            Console.WriteLine("Creating result file...");
            String outputPath = $"obsidiantools-output-analyze-{DateTime.Now:yyyyMMdd-HHmmss}.md";
            using (StreamWriter writer = File.CreateText(outputPath))
            {
                stats.Sort((a, b) => String.Compare(a.Word, b.Word, StringComparison.Ordinal));
                stats.ForEach(s =>
                {
                    writer.WriteLine($"### {s.Word}");
                    writer.WriteLine($"Counted: {s.Count}");

                    String similar = CollectionHelper.CommaSeparatedList(s.SimilarWords.Select(s => s.Word));
                    writer.WriteLine($"Similar Words: {similar}");

                    List<MarkdownLink> links = s.FileReferences.Select(MarkdownLink.ForFile).ToList();
                    String references = FileHelper.CreateMarkdownLinkList(links);
                    writer.WriteLine($"References Words: {references}");
                    writer.WriteLine("");
                    writer.WriteLine("---");
                    writer.WriteLine("");
                });
            }

            Console.WriteLine($"Creating result file done, you can find it here: {outputPath}");
        }

        private void FindSimilarWords(List<WordStatistic> stats)
        {
            Console.WriteLine("-------------");
            Console.WriteLine("Starting to search for similar words...");

            stats.ForEach(s =>
            {
                stats.ForEach(possibleMatch =>
                {
                    if (possibleMatch.Word.ToLower().Contains(s.Word.ToLower()))
                    {
                        s.SimilarWords.Add(possibleMatch);
                    }
                });
            });

            Console.WriteLine("Searching for similar words done");
            PrintStats(stats, (a, b) => a.SimilarWords.Count.CompareTo(b.SimilarWords.Count) * -1);
        }

        private void FindWordReferences(List<MarkdownFile> files, List<WordStatistic> stats)
        {
            Console.WriteLine("-------------");
            Console.WriteLine("Starting to search for word references in files...");
            foreach (MarkdownFile file in files)
            {
                try
                {
                    String text = file.Content.ToLower();
                    stats.Where(s => text.Contains(s.Word.ToLower()))
                        .ToList()
                        .ForEach(s => s.FileReferences.Add(file.Info.Name.Replace(file.Info.Extension, String.Empty)));
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred searching for file references in path '{file}'", x);
                }
            }

            Console.WriteLine("Searching for references done");
            PrintStats(stats, (a, b) => a.FileReferences.Count.CompareTo(b.FileReferences.Count) * -1);
        }

        protected override void Handle(PluginPayload payload)
        {
            IEnumerable<String> filePaths = GetMarkdownFilePaths(payload.VaultDirectory).ToList();
            List<MarkdownFile> files = ReadFiles(filePaths);
            List<WordStatistic> stats = AnalyzeMarkdownFiles(files);
            FindSimilarWords(stats);
            FindWordReferences(files, stats);
            CreateResultFile(stats);
        }

        private void HandleFile(MarkdownFile file)
        {
            // normalize text
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            String text = file.Content;
            text = Regex.Replace(text, "[^a-zA-Z0-9@äöüÄÖÜ-]+", " ", options);
            text = Regex.Replace(text, "[ ]+", " ", options);
            List<String> words = text.Split(" ").ToList();

            // filter relevant words
            List<String> processedWords = new List<String>();
            foreach (String word in words)
            {
                if (String.IsNullOrEmpty(word) || 3 > word.Length)
                {
                    continue;
                }

                processedWords.Add(word);
            }

            words = processedWords;

            // create map
            foreach (String word in words)
            {
                if (_wordStatistics.ContainsKey(word))
                {
                    _wordStatistics[word].Count++;
                }
                else
                {
                    _wordStatistics[word] = new WordStatistic
                    {
                        Count = 1
                        , FileReferences = new List<String>()
                        , SimilarWords = new List<WordStatistic>()
                        , Word = word
                    };
                }
            }
        }

        private void PrintStats(List<WordStatistic> stats, Comparison<WordStatistic> sort)
        {
            Console.WriteLine("-------------");
            Int32 takeAmount = 10;
            Console.WriteLine($"Top {takeAmount}");
            stats.Sort(sort);
            stats.Take(takeAmount).ToList().ForEach(s => Console.WriteLine($" - {s}"));
        }

        private class WordStatistic
        {
            public String Word { get; set; }
            public Int32 Count { get; set; }
            public List<WordStatistic> SimilarWords { get; set; }
            public List<String> FileReferences { get; set; }

            public override String ToString()
            {
                String similarWords = CollectionHelper.CommaSeparatedList(SimilarWords.Take(3).Select(s => s.Word));
                String fileReferences = CollectionHelper.CommaSeparatedList(FileReferences.Take(3));
                return
                    $"{Word} (occurred {Count}x) ({SimilarWords.Count}x similar: {similarWords}, ..) @ ({FileReferences.Count}x referenced: {fileReferences}, ..)";
            }
        }
    }
}
