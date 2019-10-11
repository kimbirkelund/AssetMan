using System.Collections.Generic;

namespace AssetMan.Platforms
{
    public class UwpPlatform : DensitySetPlatform
    {
        private static readonly IReadOnlyDictionary<string, double> _uwpDensities = new Dictionary<string, double>
                                                                                    {
                                                                                        { "scale-80", 0.80 },
                                                                                        { "scale-100", 1.00 },
                                                                                        { "scale-140", 1.40 },
                                                                                        { "scale-180", 1.80 },
                                                                                        { "scale-240", 2.40 }
                                                                                    };

        public UwpPlatform()
            : base("UWP", _uwpDensities) { }

        protected override string GetAssetPath(string name, string extension, string qualifier, double density)
        {
            return $"{name}.{qualifier}{extension}";
        }
    }
}
