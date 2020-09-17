using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginAnalyze : BaseTest
    {
        [ Test ]
        public void Test()
        {
            PrepareFile(GetFilePathForName("Index"), "das ist ein [[Index]], find [[SubA#b]], [[SubB|abc]]");
            PrepareFile(GetFilePathForName("SubA"), "dead end, back to [[Index]]");
            PrepareFile(GetFilePathForName("SubB"), "anther one, might be interested in [[SubA]] ? or [[Hub]]");
            PrepareFile(GetFilePathForName("Hub"), "[[SubC]], [[SubD]]");
            PrepareFile(GetFilePathForName("SubC"), "nichts, nicht wie HubD");
            PrepareFile(GetFilePathForName("SubD"), "go back to [[Hub]], or [[Index]]");

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
