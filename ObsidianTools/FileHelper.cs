using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ObsidianTools
{
    public static class FileHelper
    {
        public static String CreateMarkdownLinkList(List<MarkdownLink> links)
        {
            if (null == links || 1 > links.Count)
            {
                return "<none>";
            }

            links.Sort((a, b) => String.Compare(a.ToString(), b.ToString(), StringComparison.Ordinal));
            return links.Select(f => f.ToString()).Aggregate(CollectionHelper.AggregateWithComma);
        }

        public static String GetAbsoluteFileName(String directory, String name)
        {
            if (!name.Contains("."))
            {
                name += ".md";
            }

            return Path.Join(directory, name);
        }

        public static Boolean HasContent(String path)
        {
            if (null == path)
            {
                return false;
            }

            try
            {
                if (!File.Exists(path))
                {
                    return false;
                }

                String text = File.ReadAllText(path, Encoding.UTF8);
                return !String.IsNullOrEmpty(text?.Trim());
            }
            catch (Exception x)
            {
                LogHelper.LogException($"Warning: Could read contents of file @ {path}. Assuming it exists", x);
                return true;
            }
        }

        /**
         * Deletes the file, if it exists, only if it has no content
         * returns true if the file does not exist (anymore)
         */
        public static Boolean TryDeleteEmptyFile(String file)
        {
            try
            {
                Boolean hasContents = HasContent(file);
                if (hasContents)
                {
                    return false;
                }

                File.Delete(file);
                return !File.Exists(file);
            }
            catch (Exception x)
            {
                LogHelper.LogException($"Warning: Could not delete empty file @ {file}", x);
                return false;
            }
        }
    }
}
