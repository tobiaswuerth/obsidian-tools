using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ObsidianTools.plugins;

namespace ObsidianTools
{
    internal class Program
    {
        private readonly String[] _args;

        private readonly List<Plugin> _plugins = new List<Plugin>
        {
            new PluginCount()
            , new PluginAnalyze()
            , new PluginFindReferences()
            , new PluginCreateReferences()
            , new PluginCleanupDead()
            , new PluginCleanupAssets()
            , new PluginIdentifyHotspots()
            , new PluginListDead()
            , new PluginCreateDead()
        };

        private Program(String[] args)
        {
            _args = args;
        }

        private static void Main(String[] args)
        {
            new Program(args).Run();
        }

        private void PrintPluginOverview()
        {
            Console.WriteLine("Plugins:");
            _plugins.ForEach(p => Console.WriteLine($"\t{p.ConsoleArgument}\t\t{p.Description}"));
        }

        private void Run()
        {
            if (_args.Length < 2)
            {
                Console.WriteLine("Usage: exe <vault-path> <plugin-name> [<additional1>, ...]");
                PrintPluginOverview();
                return;
            }

            String directory = _args[0].Trim();
            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Directory: '{directory}' could not be found");
                return;
            }

            String pluginName = _args[1].Trim().ToLower();
            Plugin plugin = _plugins.FirstOrDefault(p => p.ConsoleArgument.Equals(pluginName));
            if (null == plugin)
            {
                Console.WriteLine($"Plugin for argument '{pluginName}' not found");
                PrintPluginOverview();
                return;
            }

            Console.WriteLine($"Starting plugin: {plugin.Description}");
            Console.WriteLine("===========================================");
            plugin.Execute(_args, directory);
            Console.WriteLine("===========================================");
            Console.WriteLine($"Finished plugin: {plugin.Description}");
        }
    }
}
