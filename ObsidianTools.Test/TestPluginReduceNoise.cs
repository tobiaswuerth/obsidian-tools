using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using ObsidianTools.plugins;

namespace ObsidianTools.Test
{
    public class TestPluginReduceNoise : BaseTest
    {
        [ Test ]
        public void Test()
        {
            PrepareFile(GetFilePathForName("Index"), "[[a1]] [[a2]] [[b]]");
            PrepareFile(GetFilePathForName("a1"), "[[a1.1]] [[a1.2]] [[a1.3]], siehe auch: [[a2]]");
            PrepareFile(GetFilePathForName("a2"), "[[a2.1]] [[a2.2]]");
            PrepareFile(GetFilePathForName("b"), "[[b1]] [[b2]] [[b3]] [[b4]] [[b5]]");
            PrepareFile(GetFilePathForName("b1"), "[[b]] [[b1.1]]");
            PrepareFile(GetFilePathForName("b2"), "[[b]] [[b2.1]]");
            PrepareFile(GetFilePathForName("b3"), "[[b]] [[b3.1]]");
            PrepareFile(GetFilePathForName("b4"), "[[b]] [[b4.1]]");
            PrepareFile(GetFilePathForName("b5"), "[[b]] [[b5.1]]");
            PrepareFile(GetFilePathForName("b5.1"), "[[b5]] [[b5.1]]");

            new PluginReduceNoise().Execute(new[]
                {
                    VaultDirectory,
                     "b", "b"
                }
                , VaultDirectory);


        }
    }
}
