using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginAnalyze : BaseTest
    {
        [ Test ]
        public void Test()
        {
            PrepareFile(FilePathForName("Index"), "das ist ein [[Index]], find [[SubA#b]], [[SubB|abc]]");
            PrepareFile(FilePathForName("SubA"), "dead end, back to [[Index]]");
            PrepareFile(FilePathForName("SubB"), "anther one, might be interested in [[SubA]] ? or [[Hub]]");
            PrepareFile(FilePathForName("Hub"), "[[SubC]], [[SubD]]");
            PrepareFile(FilePathForName("SubC"), "nichts, nicht wie HubD");
            PrepareFile(FilePathForName("SubD"), "go back to [[Hub]], or [[Index]]");

            new PluginAnalyze().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            MarkdownFile file = GetPluginOutputFile();
            Assert.IsTrue(file.IsHealthy());
            Assert.AreEqual(28, file.Links.Count);
        }
    }
}
