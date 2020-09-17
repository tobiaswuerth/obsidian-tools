using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace ObsidianTools.Test
{
    public abstract class BaseTest
    {
        protected static readonly String TestDirectory =
            Path.Join(Environment.CurrentDirectory, $"\\_tmp_ObsidianTool-Test-Files-{Guid.NewGuid()}");

        protected static readonly String VaultDirectory = Path.Join(TestDirectory, "\\vault");
        private readonly String _directoryBefore = Environment.CurrentDirectory;

        protected static String RandomFilePath
        {
            get
            {
                String randomString = Guid.NewGuid().ToString("N");
                String fileName = $"ObsidianTool-Test-{randomString}.md";
                return Path.Join(VaultDirectory, fileName);
            }
        }

        protected void CleanupFile(String path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return;
                }

                File.Delete(path);
                if (File.Exists(path))
                {
                    Assert.Fail("Cannot cleanup file");
                }
            }
            catch (Exception x)
            {
                Assert.Fail("Cannot cleanup file", x);
            }
        }

        protected static String FilePathForName(String name)
        {
            return Path.Join(VaultDirectory, $"{name}.md");
        }

        protected MarkdownFile GetPluginOutputFile()
        {
            String[] markdownFiles = Directory.GetFiles(TestDirectory, "*.md", SearchOption.TopDirectoryOnly);
            String outputFile = 0 < markdownFiles.Length ? markdownFiles[0] : String.Empty;
            if (!String.IsNullOrEmpty(outputFile))
            {
                return new MarkdownFile(outputFile);
            }

            Assert.Fail("Output file not found");
            return null;
        }

        protected void PrepareFile(String path, String content)
        {
            try
            {
                File.WriteAllText(path, content, Encoding.UTF8);
                String isText = File.ReadAllText(path, Encoding.UTF8);
                if (!content.Equals(isText))
                {
                    Assert.Fail("File content not correct");
                }
            }
            catch (Exception x)
            {
                CleanupFile(path);
                Assert.Fail("Cannot prepare file", x);
            }
        }

        [ SetUp ]
        public void SetUp()
        {
            if (!Directory.Exists(TestDirectory))
            {
                Directory.CreateDirectory(TestDirectory);
            }

            Directory.CreateDirectory(VaultDirectory);
            Environment.CurrentDirectory = TestDirectory;
        }

        [ TearDown ]
        public void TearDown()
        {
            Environment.CurrentDirectory = _directoryBefore;
            if (Directory.Exists(TestDirectory))
            {
                Directory.Delete(TestDirectory, true);
            }
        }
    }
}
