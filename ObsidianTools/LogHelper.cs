using System;

namespace ObsidianTools
{
    public static class LogHelper
    {
        public static void LogException(String prefix, Exception x)
        {
            if (null == x)
            {
                return;
            }

            prefix = prefix?.Trim() ?? String.Empty;
            if (!String.IsNullOrEmpty(prefix))
            {
                Console.Error.WriteLine(prefix);
            }

            Console.Error.WriteLine($"Message: {x.Message}");
            Console.Error.WriteLine($"Stacktrace: {x.StackTrace}");
            Console.Error.WriteLine($"Source: {x.Source}");
            Console.Error.WriteLine($"TargetSite: {x.TargetSite}");
            Console.Error.WriteLine($"HelpLink: {x.HelpLink}");
            if (null != x.InnerException)
            {
                Console.Error.WriteLine("+ Inner Exception:");
                LogException(null, x.InnerException);
            }
        }
    }
}
