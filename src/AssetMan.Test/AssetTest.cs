using Xunit;

namespace AssetMan.Test
{
    public class AssetTest
    {
        [Fact]
        public void Export()
        {
            using (var asset = new Asset("../../../Images/logo@xxxhdpi.png"))
                asset.Export("Images", 100, 100);
        }
    }
}
