using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetMan.Assets;
using AssetMan.Extensions;

namespace AssetMan.Platforms
{
    public class IosPlatform : DensitySetPlatform
    {
        private const string ContentsItemTemplate = @"{{ ""filename"": ""{0}"", ""scale"": ""{1}x"", ""idiom"": ""universal"" }}";

        private const string ContentsTemplate = @"{{ ""images"": [ {0} ], ""info"": {{ ""version"": 1, ""author"": ""xcode"" }} }}";

        private static readonly Dictionary<string, double> _iosDensities = new Dictionary<string, double>
                                                                           {
                                                                               { "", 1 },
                                                                               { "@2x", 2 },
                                                                               { "@3x", 3 }
                                                                           };

        public IosPlatform()
            : base("iOS", _iosDensities) { }

        public override void Export(IAsset asset, string folder)
        {
            base.Export(asset, folder);
            GenerateContents(asset, folder);
        }

        protected override string GetAssetPath(string name, string extension, string qualifier, double density)
        {
            var imagesetFolder = $"{name}.imageset";
            var imageFile = CreateAssetFilename(name, extension, qualifier);
            return Path.Combine(imagesetFolder, imageFile);
        }

        private void GenerateContents(IAsset asset, string folder)
        {
            var imagesetFolder = Path.Combine(folder, $"{asset.FilenameWithoutQualifierAndExtension}.imageset");

            // Writing Contents.json
            var contentFile = Path.Combine(imagesetFolder, "Contents.json");
            var densities = Densities.Where(x => x.Value <= asset.Density);
            var images = string.Join(", ",
                                     densities.Select(d => string.Format(ContentsItemTemplate,
                                                                         CreateAssetFilename(asset.FilenameWithoutQualifierAndExtension, asset.Extension, d.Key),
                                                                         d.Value.ToString("0.##"))));

            contentFile.CreateParentDirectory();

            File.WriteAllText(contentFile, string.Format(ContentsTemplate, images));
            Log.Write($"AssetManGeneratedFile({contentFile})");
        }

        private static string CreateAssetFilename(string name, string extension, string qualifier)
        {
            return $"{name}{qualifier}{extension}";
        }
    }
}
