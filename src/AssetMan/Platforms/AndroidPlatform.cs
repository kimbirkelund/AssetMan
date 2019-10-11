using System.Collections.Generic;
using System.IO;

namespace AssetMan.Platforms
{
    public class AndroidPlatform : DensitySetPlatform
    {
        private static readonly IReadOnlyDictionary<string, double> _androidDensities = new Dictionary<string, double>
                                                                                        {
                                                                                            { "ldpi", 0.75 },
                                                                                            { "mdpi", 1.00 },
                                                                                            { "hdpi", 1.50 },
                                                                                            { "xhdpi", 2.00 },
                                                                                            { "xxhdpi", 3.00 },
                                                                                            { "xxxhdpi", 4.00 }
                                                                                        };

        public AndroidPlatform()
            : base("Android", _androidDensities) { }

        protected override string GetAssetPath(string name, string extension, string qualifier, double density)
        {
            return Path.Combine($"drawable-{qualifier}", $"{name}{extension}");
        }
    }
}
