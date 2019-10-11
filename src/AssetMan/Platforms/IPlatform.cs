using AssetMan.Assets;

namespace AssetMan.Platforms
{
    public interface IPlatform
    {
        string Name { get; }

        void Export(IAsset asset, string folder);
    }
}
