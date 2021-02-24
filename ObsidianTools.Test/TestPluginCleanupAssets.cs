using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginCleanupAssets : BaseTest
    {
        [ Test ]
        public void Test()
        {
            PrepareFile(Path.Join(VaultDirectory, "index.md"), "this is a test for ![[image askdl - 27.png]] image embedding");
            PrepareFile(Path.Join(VaultDirectory, "image askdl - 27.png"), "a");
            PrepareFile(Path.Join(VaultDirectory, "image2.png"), "b");
            PrepareFile(Path.Join(VaultDirectory, "image 3 more.png"), "d");
            PrepareFile(Path.Join(VaultDirectory, "style.css"), "d");
            new PluginCleanupAssets().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            String targetDir = Path.Join(VaultDirectory, "_UNUSED");
            Assert.IsTrue(Directory.Exists(targetDir));
            String[] files = Directory.GetFiles(targetDir);
            Assert.AreEqual(2, files.Length);
            Assert.IsTrue(files.Contains(Path.Join(targetDir, "image2.png")));
            Assert.IsFalse(files.Contains(Path.Join(targetDir, "style.css")));
        }
    }
}
