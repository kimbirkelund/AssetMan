using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions;
using NCrunch.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AssetMan.Test
{
    public class OptionsTest
    {
        [Fact]
        public void TestLoadOptions()
        {
            var optionsFile = Path.Combine(GetProjectFolder(), "Config.assets.json");
            dynamic expected = JObject.Load(new JsonTextReader(new StreamReader(optionsFile)));

            var options = Options.Load(optionsFile);
            options.Should()
                   .NotBeNull();

            options.Platform.Should()
                   .Be((string)expected.Platform);

            options.Output.Should()
                   .Be(Path.Combine(Path.GetDirectoryName(optionsFile), (string)expected.Output));

            options.Input.Should()
                   .BeEquivalentTo(((JArray)expected.Input).Select(i => Path.Combine(Path.GetDirectoryName(optionsFile), i.Value<string>())));
        }

        private static string GetProjectFolder([CallerFilePath] string callerFilePath = null)
        {
            if (NCrunchEnvironment.NCrunchIsResident())
                return Path.GetDirectoryName(NCrunchEnvironment.GetOriginalProjectPath());

            return Path.GetDirectoryName(callerFilePath);
        }
    }
}
