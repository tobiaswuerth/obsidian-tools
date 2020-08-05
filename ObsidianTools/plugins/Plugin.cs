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

        public void Execute(String[] args, String path)
        {
            IEnumerable<String> markdownFiles = Directory.EnumerateFiles(path, "*.md", SearchOption.AllDirectories);
            Handle(args, markdownFiles);
        }

        protected abstract void Handle(String[] args, IEnumerable<String> files);
    }
}
