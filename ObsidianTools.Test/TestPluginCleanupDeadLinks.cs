using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginCleanupDeadLinks : BaseTest
    {
        [ Test ]
        public void Test()
        {
            const String TEXT_BEFORE =
                "pre [[a]], [[b#c]], [[d|e]], [[f#g|h]]\r\n[[Index]], [[Index#2]], [[Index|index]], [[Index#3|yea]] suf";
            const String TEXT_AFTER = "pre a, b#c, e, h\r\n[[Index]], [[Index#2]], [[Index|index]], [[Index#3|yea]] suf";

            String path = Path.Join(VaultDirectory, "Index.md");
            PrepareFile(path, TEXT_BEFORE);

            new PluginCleanup().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            String isText = File.ReadAllText(path, Encoding.UTF8);
            if (!isText.Equals(TEXT_AFTER))
            {
                Assert.Fail("text not properly cleaned up");
            }
        }
    }
}
