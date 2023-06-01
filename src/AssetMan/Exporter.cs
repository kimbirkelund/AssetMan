using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetMan.Assets;
using AssetMan.Platforms;

namespace AssetMan
{
    public class Exporter
    {
        private readonly List<IPlatform> _platforms = new List<IPlatform>();

        public string[] SupportedExtensions { get; set; } = { ".png", ".jpg" };

        public Exporter()
        {
            Register(new IosPlatform());
            Register(new AndroidPlatform());
        }

        public void Export(Options configuration)
        {
            var platform = _platforms.FirstOrDefault(x => x.Name == configuration.Platform);

            if (platform == null)
                throw new NullReferenceException("Platform not found");

            var assetPaths = configuration.Input
                                          .SelectMany(Directory.GetFiles)
                                          .Where(x => SupportedExtensions.Contains(Path.GetExtension(x.ToLower())));

            foreach (var path in assetPaths)
            {
                using (var asset = new Asset(path))
                    platform.Export(asset, configuration.Output);
            }
        }

        public Exporter Register(IPlatform platform)
        {
            _platforms.Add(platform);
            return this;
        }
    }
}
