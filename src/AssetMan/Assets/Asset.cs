using System;
using System.IO;
using AssetMan.Extensions;
using SkiaSharp;

namespace AssetMan.Assets
{
    public class Asset : IAsset
    {
        private readonly SKBitmap bitmap;

        public double Density { get; set; } = 1.0;

        public string Extension { get; }

        public string FilenameWithoutQualifierAndExtension { get; }

        public string Path { get; }

        public Asset(string path)
        {
            Path = path;
            Path.WithoutQualifier(out var name, out var extension);
            FilenameWithoutQualifierAndExtension = name;
            Extension = extension;

            if (Densities.TryFind(path, out var foundDensity))
                Density = foundDensity;

            using (var input = File.Open(path, FileMode.Open))
            using (var memory = new MemoryStream())
            {
                input.CopyTo(memory);
                memory.Seek(0, SeekOrigin.Begin);
                memory.Position = 0;

                using (var stream = new SKManagedStream(memory))
                {
                    bitmap = SKBitmap.Decode(stream);

                    if (bitmap == null)
                        throw new InvalidOperationException($"The provided image isn't valid : {path} (SKBitmap can't be created).");
                }
            }
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }

        public void Export(string path, int width, int height)
        {
            if (!File.Exists(path) || File.GetLastWriteTime(path) < File.GetLastWriteTime(Path))
            {
                Log.Write($"[{Path} ({bitmap.Width}x{bitmap.Height})({Density}x)] -> Generating [{path} ({width}x{height})]");

                // SKBitmap.Resize() doesn't support SKColorType.Index8
                // https://github.com/mono/SkiaSharp/issues/331
                if (bitmap.ColorType != SKImageInfo.PlatformColorType)
                    bitmap.CopyTo(bitmap, SKImageInfo.PlatformColorType);

                var info = new SKImageInfo(width, height);

                using (var resized = bitmap.Resize(info, SKFilterQuality.High))
                {
                    if (resized == null)
                        throw new InvalidOperationException($"Failed to resize : {Path}.");

                    using (var image = SKImage.FromBitmap(resized))
                    using (var data = image.Encode())
                    {
                        path.CreateParentDirectory();

                        if (File.Exists(path))
                            File.Delete(path);

                        using (var fileStream = File.Create(path))
                        using (var outputStream = data.AsStream())
                            outputStream.CopyTo(fileStream);
                    }
                }
            }
            else
                Log.Write($"[{Path} ({bitmap.Width}x{bitmap.Height})({Density}x)] -> Didn't generate [{path} ({width}x{height})] because it already exists.");

            Log.Write($"AssetManGeneratedFile({path})");
        }

        public void Export(string path, double density)
        {
            var densityFactor = density / Density;
            var width = (int)Math.Ceiling(bitmap.Width * densityFactor);
            var height = (int)Math.Ceiling(bitmap.Height * densityFactor);
            Export(path, width, height);
        }
    }
}
