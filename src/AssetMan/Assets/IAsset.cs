using System;

namespace AssetMan.Assets
{
    public interface IAsset : IDisposable
    {
        double Density { get; }
        string Extension { get; }
        string FilenameWithoutQualifierAndExtension { get; }
        string Path { get; }

        void Export(string path, double density);
        void Export(string path, int width, int height);
    }
}
