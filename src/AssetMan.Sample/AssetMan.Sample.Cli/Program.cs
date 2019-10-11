using System.Diagnostics;
using System.IO;
using System.Reflection;
using AssetMan.Assets;

namespace AssetMan.Sample.Cli
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var root = Path.GetDirectoryName(Assembly.GetEntryAssembly()
                                                     ?.Location);
            Debug.Assert(root != null, nameof(root) + " != null");

            using (var asset = new Asset(Path.Combine(root, "Images/ic_warning@3x.png")))
                asset.Export(Path.Combine(root, "Output/output.png"), 100, 100);

            var exporter = new Exporter();

            exporter.Export(new Options("iOS", Path.Combine(root, "Output/iOS/"), new[] { Path.Combine(root, "Images/") }));

            exporter.Export(new Options("Android", Path.Combine(root, "Output/Android/"), new[] { Path.Combine(root, "Images/") }));
        }
    }
}
