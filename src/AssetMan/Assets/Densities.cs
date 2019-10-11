using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AssetMan.Assets
{
    public static class Densities
    {
        public static readonly Regex ExplicitNaming = new Regex(@"\@([0-9]+(.[0-9]+)?)x\.[a-zA-Z]+$", RegexOptions.Compiled);

        public static readonly IReadOnlyDictionary<string, double> KnownQualifiedNames = new Dictionary<string, double>
                                                                                         {
                                                                                             { "ldpi", 0.75 },
                                                                                             { "mdpi", 1.00 },
                                                                                             { "hdpi", 1.50 },
                                                                                             { "xhdpi", 2.00 },
                                                                                             { "xxhdpi", 3.00 },
                                                                                             { "xxxhdpi", 4.00 }
                                                                                         };

        public static readonly Regex QualifiedNaming = new Regex(@"\@([a-z]+)\.[a-zA-Z]+$", RegexOptions.Compiled);

        public static bool TryFind(string path, out double density)
        {
            var explicitNamingMatch = ExplicitNaming.Match(path);
            if (explicitNamingMatch.Success)
            {
                density = double.Parse(explicitNamingMatch.Groups[1]
                                                          .Value);
                return true;
            }

            var qualifiedNamingMatch = QualifiedNaming.Match(path);
            if (qualifiedNamingMatch.Success
                && KnownQualifiedNames.TryGetValue(qualifiedNamingMatch.Groups[1]
                                                                       .Value,
                                                   out density))
                return true;

            density = 1.0;
            return false;
        }
    }
}
