using System;
using System.IO;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginCleanupEmptyFiles : BaseTest
    {
        [ Test ]
        public void Test()
        {
            String path = RandomFilePath;
            PrepareFile(path, " \n  \r \t   ");

            new PluginCleanupDead().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            if (File.Exists(path))
            {
                Assert.Fail("file not properly deleted");
            }
        }
    }
}
