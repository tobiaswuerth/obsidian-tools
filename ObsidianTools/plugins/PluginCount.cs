using System;
using System.Linq;

namespace ObsidianTools.plugins
{
    public class PluginCount : Plugin
    {
        public PluginCount() : base("--count", "Counts all markdown files") { }
        public Int32 LinkCount { get; private set; }

        protected override void Handle(PluginPayload payload)
        {
            LinkCount = GetMarkdownFilePaths(payload.VaultDirectory).Count();
            Console.WriteLine($"Found {LinkCount} markdown files in given directory");
        }
    }
}
