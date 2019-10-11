using System;
using System.IO;
using AssetMan.Extensions;
using JetBrains.Annotations;
using SkiaSharp;

namespace AssetMan.Assets
{
    public class Asset : IAsset
    {
        private readonly SKBitmap _bitmap;

        public double Density { get; } = 1.0;
        public string Extension { get; }
        public string FilenameWithoutQualifierAndExtension { get; }
        public string Path { get; }

        public Asset([NotNull] string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));

            (FilenameWithoutQualifierAndExtension, Extension) = Path.WithoutQualifier();

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
                    _bitmap = SKBitmap.Decode(stream);

                    if (_bitmap == null)
                        throw new InvalidOperationException($"The provided image isn't valid: {path} (SKBitmap can't be created).");
                }
            }
        }

        public void Dispose()
        {
            _bitmap.Dispose();
        }

        public void Export(string path, int width, int height)
        {
            if (File.Exists(path) && File.GetLastWriteTime(path) >= File.GetLastWriteTime(Path))
                Log.Write($"[{Path} ({_bitmap.Width}x{_bitmap.Height})({Density}x)] -> Didn't generate [{path} ({width}x{height})] because it already exists and hasn't been updated.");
            else
            {
                Log.Write($"[{Path} ({_bitmap.Width}x{_bitmap.Height})({Density}x)] -> Generating [{path} ({width}x{height})].");

                if (_bitmap.ColorType != SKImageInfo.PlatformColorType)
                    _bitmap.CopyTo(_bitmap, SKImageInfo.PlatformColorType);

                var info = new SKImageInfo(width, height);

                using (var resized = _bitmap.Resize(info, SKFilterQuality.High))
                {
                    if (resized == null)
                        throw new InvalidOperationException($"Failed to resize: {Path}.");

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

            Log.Write($"AssetManGeneratedFile({path})");
        }

        public void Export(string path, double density)
        {
            var densityFactor = density / Density;
            var width = (int)Math.Ceiling(_bitmap.Width * densityFactor);
            var height = (int)Math.Ceiling(_bitmap.Height * densityFactor);

            Export(path, width, height);
        }
    }
}
