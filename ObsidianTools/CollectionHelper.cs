using System;
using System.Collections.Generic;
using System.Linq;

namespace ObsidianTools
{
    public static class CollectionHelper
    {
        public static String AggregateWithComma(String a, String b)
        {
            return $"{a}, {b}";
        }

        public static String CommaSeparatedList(IEnumerable<String> eEntries)
        {
            List<String> entries = eEntries?.ToList() ?? new List<String>();
            if (1 > entries.Count)
            {
                return "<none>";
            }

            if (2 > entries.Count)
            {
                return entries.First();
            }

            return entries.Aggregate(AggregateWithComma);
        }
    }
}
