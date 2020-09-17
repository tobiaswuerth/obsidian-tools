using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginCreateReferences : BaseTest
    {
        [ Test ]
        public void Test()
        {
            List<(String Name, String Before, String After)> files = new List<(String, String, String)>
            {
                ("Index", "das hier ist der Index. ich habe Projekte und Management Aufgaben"
                    , "das hier ist der [[Index]]. ich habe [[Projekt]]e und Management Aufgaben")
                , ("Projekt", "ein Unterfangen, AnotherDocument welches vom Manager oder der Managerin geleitet wird"
                    , "ein Unterfangen, [[AnotherDocument]] welches vom [[Manager]] oder der [[Manager]]in geleitet wird")
                , ("Manager", "chef, siehe Index, leitet Projekte, Projektmanager auch Projektmanagement, Seniormanager"
                    , "chef, siehe [[Index]], leitet [[Projekt]]e, [[Projektmanager]] auch [[Projekt]]management, Senior[[Manager|manager]]")
                , ("AnotherDocument", "chef, siehe Index,[[ leitet Projekte]], auch Projektmanagement, Seniormanager"
                    , "chef, siehe [[Index]],[[ leitet Projekte]], auch [[Projekt]]management, Senior[[Manager|manager]]")
                , ("Projektmanager", "chef, siehe [[Index, leitet Projekte, auch Projektmanagement]], SENIORMANAGER"
                    , "chef, siehe [[Index, leitet Projekte, auch Projektmanagement]], SENIOR[[Manager|MANAGER]]")
            };

            files.ForEach(f => PrepareFile(GetFilePathForName(f.Name), f.Before));

            new PluginCreateReferences().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            files.ForEach(f =>
            {
                String path = Path.Combine(VaultDirectory, $"{f.Name}.md");
                String content = File.ReadAllText(path, Encoding.UTF8);
                Assert.AreEqual(f.After, content);
            });
        }

        [ Test ]
        public void Test_Links()
        {
            List<(String Name, String Before, String After)> files = new List<(String, String, String)>
            {
                ("Index", "this is Job anjobother Room aroom- [Job Room](https://www.job-room.ch/) url test blub"
                    , "this is [[Job]] an[[Job|job]]other [[Room]] a[[Room|room]]- [Job Room](https://www.job-room.ch/) url test blub")
                , ("Room", "a", "a")
                , ("Job", "c", "c")
            };

            files.ForEach(f => PrepareFile(GetFilePathForName(f.Name), f.Before));

            new PluginCreateReferences().Execute(new[]
                {
                    VaultDirectory
                }
                , VaultDirectory);

            files.ForEach(f =>
            {
                String path = Path.Combine(VaultDirectory, $"{f.Name}.md");
                String content = File.ReadAllText(path, Encoding.UTF8);
                Assert.AreEqual(f.After, content);
            });
        }
    }
}
