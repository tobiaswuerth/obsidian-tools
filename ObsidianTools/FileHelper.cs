using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ObsidianTools
{
    public static class FileHelper
    {
        public static String CreateFileReference(String fileName)
        {
            return $"[[{fileName}]]";
        }

        public static String CreateFileReferenceList(List<String> files)
        {
            if (null == files || 1 > files.Count)
            {
                return "<none>";
            }

            files.Sort();
            return files.Select(CreateFileReference).Aggregate(CollectionHelper.AggregateWithComma);
        }

        public static String GetFileNameWithoutExtension(String fullName)
        {
            if (null == fullName || !File.Exists(fullName))
            {
                return String.Empty;
            }

            try
            {
                FileInfo i = new FileInfo(fullName);
                String name = i.Name.Replace(i.Extension, "");
                return name;
            }
            catch (Exception x)
            {
                LogHelper.LogException($"Warning: Could not get file info for file @ {fullName}", x);
                return String.Empty;
            }
        }

        /**
         * Deletes the file, if it exists, only if it has no content
         * returns true if the file does not exist (anymore)
         */
        public static Boolean TryDeleteEmptyFile(String file)
        {
            if (null == file)
            {
                return false;
            }

            try
            {
                if (!File.Exists(file))
                {
                    return true;
                }

                String text = File.ReadAllText(file, Encoding.UTF8);
                if (!String.IsNullOrEmpty(text?.Trim()))
                {
                    return false;
                }

                // seems to be an existing, empty file
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
