using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginCreateDeadLinks : BaseTest
    {
        [ Test ]
        public void Test()
        {
            String path = Path.Join(VaultDirectory, "Index.md");
            PrepareFile(path
                , "pre [[a]], [[b#c]], [[d|e]], [[f#g|h]]\r\n[[Index]], [[Index#2]], [[Index|index]], [[Index#3|yea]] suf");

            List<String> targetFiles = new List<String>
            {
                Path.Join(VaultDirectory, "a.md")
                , Path.Join(VaultDirectory, "b.md")
                , Path.Join(VaultDirectory, "d.md")
                , Path.Join(VaultDirectory, "f.md")
            };

            targetFiles.ForEach(f => Assert.IsFalse(File.Exists(f)));

            new PluginCreateDead().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            targetFiles.ForEach(f => Assert.IsTrue(File.Exists(f)));
        }
    }
}
