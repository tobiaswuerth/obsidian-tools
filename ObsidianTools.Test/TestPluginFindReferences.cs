using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginFindReferences : BaseTest
    {
        [ Test ]
        public void Test()
        {
            PrepareFile(RandomFilePath, "pre this is a sample project aprojekt [[a]], [[ndex]], [[Index#3|yea]] suf");
            PrepareFile(RandomFilePath, "hier haben wir ei anderes projekt, inkl. projektmanagement");
            PrepareFile(RandomFilePath, "dieser hat nichts als referenz");

            new PluginFindReferences().Execute(new[]
                {
                    VaultDirectory
                    , "--find-references"
                    , "project"
                    , "Projekt"
                }
                , VaultDirectory);

            MarkdownFile file = GetPluginOutputFile();
            Assert.IsTrue(file.IsHealthy());
            Assert.AreEqual(2, file.Links.Count);
        }
    }
}
