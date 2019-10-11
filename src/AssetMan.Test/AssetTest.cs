using System;
using System.IO;
using FluentAssertions;
using SkiaSharp;
using Xunit;

namespace AssetMan.Test
{
    public class AssetTest
    {
        [Fact]
        public void Export()
        {
            var inputPath = CreateInputFile();
            var outputPath = GetTempPath();

            using (var asset = new Asset(inputPath))
                asset.Export(outputPath, 100, 100);

            File.Exists(outputPath)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Export_ShouldReplaceOlderFiles()
        {
            var inputPath = CreateInputFile();
            var outputPath = GetTempPath();

            using (var asset = new Asset(inputPath))
                asset.Export(outputPath, 100, 100);

            File.Exists(outputPath)
                .Should()
                .BeTrue();

            var outputPathLastWriteTime = File.GetLastWriteTime(outputPath);


            using (var asset = new Asset(inputPath))
                asset.Export(outputPath, 100, 100);

            File.GetLastWriteTime(outputPath)
                .Should()
                .Be(outputPathLastWriteTime);


            File.SetLastWriteTime(inputPath,
                                  File.GetLastWriteTime(outputPath)
                                      .AddSeconds(1));

            using (var asset = new Asset(inputPath))
                asset.Export(outputPath, 100, 100);

            File.GetLastWriteTime(outputPath)
                .Should()
                .BeAfter(outputPathLastWriteTime);
        }

        private static string CreateInputFile()
        {
            var inputPath = GetTempPath();

            using (var stream = File.OpenWrite(inputPath))
            {
                SKBitmap.FromImage(SKImage.Create(new SKImageInfo(100, 100)))
                        .PeekPixels()
                        .Encode(SKPngEncoderOptions.Default)
                        .SaveTo(stream);
            }

            return inputPath;
        }

        private static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}");
        }
    }
}
