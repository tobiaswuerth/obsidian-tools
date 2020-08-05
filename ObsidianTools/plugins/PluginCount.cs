using System;
using System.Collections.Generic;
using System.Linq;

namespace ObsidianTools.plugins
{
    public class PluginCount : Plugin
    {
        public PluginCount() : base("--count", "Counts all markdown files") { }

        protected override void Handle(String[] args, IEnumerable<String> files)
        {
            Int32 count = files.Count();
            Console.WriteLine($"Found {count} markdown files in given directory");
        }
    }
}
