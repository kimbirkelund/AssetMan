using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetMan.Assets;

namespace AssetMan.Platforms
{
    public abstract class DensitySetPlatform : IPlatform
    {
        public IReadOnlyDictionary<string, double> Densities { get; }

        public string Name { get; }

        protected DensitySetPlatform(string name, IReadOnlyDictionary<string, double> densities)
        {
            Name = name;
            Densities = densities;
        }

        public virtual void Export(IAsset asset, string folder)
        {
            foreach (var density in Densities.Where(x => x.Value <= asset.Density))
            {
                Log.Write($"Density '{density.Key}' = {density.Value}x");

                var assetPath = GetAssetPath(asset.FilenameWithoutQualifierAndExtension, asset.Extension, density.Key, density.Value);
                var path = Path.Combine(folder, assetPath);

                asset.Export(path, density.Value);
            }
        }

        protected abstract string GetAssetPath(string name, string extension, string qualifier, double density);
    }
}
