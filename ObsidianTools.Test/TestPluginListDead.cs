using System;
using System.IO;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginListDead : BaseTest
    {
        [ Test ]
        public void Test()
        {
            String path = Path.Join(VaultDirectory, "Index.md");
            PrepareFile(path
                , "pre [[a]], [[b#c]], [[d|e]], [[f#g|h]]\r\n[[Index]], [[a.b.c]] [[Index#2]], [[Index|index]], [[Index#3|yea]] suf");
            PrepareFile(GetFilePathForName("a.b.c"), "abc");
            new PluginListDead().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            MarkdownFile file = GetPluginOutputFile();
            Assert.IsTrue(file.IsHealthy());
            Assert.AreEqual(8, file.Links.Count);
        }
    }
}
