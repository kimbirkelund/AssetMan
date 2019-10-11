using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AssetMan
{
    public class Options
    {
        public IReadOnlyCollection<string> Input { get; private set; }
        public string Output { get; private set; }
        public string Platform { get; }

        public Options(string platform, string output, IEnumerable<string> input)
        {
            Platform = platform;
            Output = output;
            Input = input?.ToList() ?? new List<string>();
        }

        public static Options Load([NotNull] string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var json = File.ReadAllText(path);
            var options = JsonConvert.DeserializeObject<Options>(json);

            var parent = Path.GetDirectoryName(path);
            Debug.Assert(parent != null, nameof(parent) + " != null");

            options.Output = Path.Combine(parent, options.Output);
            options.Input = options.Input.Select(x => Path.Combine(parent, x))
                                   .ToArray();

            return options;
        }
    }
}
