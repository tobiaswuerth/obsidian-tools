using System;
using System.Collections.Generic;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginCount : BaseTest
    {
        [ Test ]
        public void Test()
        {
            List<String> paths = new List<String>
            {
                RandomFilePath
                , RandomFilePath
                , RandomFilePath
            };

            paths.ForEach(p => PrepareFile(p, "some [[a]], [[b#c]] `yea` #()!content"));

            PluginCount plugin = new PluginCount();
            plugin.Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            Assert.AreEqual(3, plugin.LinkCount);
        }
    }
}
