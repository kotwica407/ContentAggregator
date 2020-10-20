using System.Collections.Generic;
using System.Linq;

namespace ContentAggregator.Services.Helpers
{
    internal static class TagHelpers
    {
        internal static string[] GetTagsFromText(string text)
        {
            List<string> result = new List<string>();
            string[] parts = text.Split(null); //white-space is assumed to be the splitting character
            foreach (string part in parts)
            {
                int firstPositionOfHash = part.IndexOf('#');
                switch (firstPositionOfHash)
                {
                    case -1:
                        continue;
                    default:
                        result.AddRange(part.Split('#').Skip(1));
                        break;
                }
            }

            return result.Distinct().ToArray();
        }
    }
}