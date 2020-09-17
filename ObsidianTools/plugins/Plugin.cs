using System;
using System.Collections.Generic;
using System.IO;

namespace ObsidianTools.plugins
{
    public abstract class Plugin
    {
        public readonly String ConsoleArgument;
        public readonly String Description;

        protected Plugin(String consoleArgument, String description)
        {
            ConsoleArgument = consoleArgument;
            Description = description;
        }

        public void Execute(String[] cliArgs, String vaultDirectory)
        {
            Handle(new PluginPayload
            {
                ConsoleArguments = cliArgs
                , VaultDirectory = vaultDirectory
            });
        }

        protected IEnumerable<String> GetMarkdownFilePaths(String directory)
        {
            return Directory.EnumerateFiles(directory, "*.md", SearchOption.AllDirectories);
        }

        protected abstract void Handle(PluginPayload payload);

        protected List<MarkdownFile> ReadFiles(IEnumerable<String> paths)
        {
            List<MarkdownFile> content = new List<MarkdownFile>();

            Console.WriteLine("Loading files into memory...");
            foreach (String path in paths)
            {
                try
                {
                    content.Add(new MarkdownFile(path));
                }
                catch (Exception x)
                {
                    LogHelper.LogException($"An error occurred reading file '{path}'", x);
                }
            }

            Console.WriteLine("Files loaded");

            return content;
        }
    }
}
