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
                "pre [[a]], [[b#c]], [[d|e]], [[f#g|h]]\r\n[[Index]], [[Index#2]], [[Index|index]], [[Index#3|yea]] suf ![[embedded]] [[image2.png|text]] [[image3.png]] [[image4.png|text2]] [[image5.png]]";
            const String TEXT_AFTER =
                "pre a, b#c, e, h\r\n[[Index]], [[Index#2]], [[Index|index]], [[Index#3|yea]] suf ![[embedded]] [[image2.png|text]] [[image3.png]] text2 image5.png";

            String path = GetFilePathForName("Index");
            PrepareFile(path, TEXT_BEFORE);
            PrepareFile(Path.Join(VaultDirectory, "image2.png"), "a");
            PrepareFile(Path.Join(VaultDirectory, "image3.png"), "a");
            new PluginCleanupDead().Execute(new[]
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
