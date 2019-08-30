using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace AssetMan.Tasks
{
    public class ExportAssets : ToolTask
    {
        private static readonly Regex _outputRegex = new Regex(@"\[(?<sourceAsset>.+) \(\d+x\d+\)\([^)]+\)\] ->[^[]+\[(?<generatedAsset>.+) \(\d+x\d+\)\]",
                                                               RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly List<string> _generatedAssets = new List<string>();
        private readonly List<string> _sourceAssets = new List<string>();

        public string AssetManCliPath { get; set; }

        public bool Debug { get; set; }

        [Output]
        public string[] GeneratedAssets => _generatedAssets.ToArray();

        [Output]
        public string[] SourceAssetFolders => _sourceAssets.Select(Path.GetDirectoryName)
                                                           .Distinct()
                                                           .ToArray();

        protected override MessageImportance StandardOutputLoggingImportance => MessageImportance.Normal;

        protected override string ToolName { get; } = "AssetMan.Cli";

        public override bool Execute()
        {
            if (Debug)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                else
                    Debugger.Launch();
            }

            return base.Execute();
        }

        protected override string GenerateCommandLineCommands()
        {
            var optionsFiles = Directory.GetFiles(GetWorkingDirectory() ?? Environment.CurrentDirectory, "*.assets.json");

            var builder = new CommandLineBuilder();

            if (AssetManCliPath.EndsWith("dll"))
                builder.AppendFileNameIfNotNull(AssetManCliPath);

            builder.AppendSwitchIfNotNull("-Options ", string.Join(";", optionsFiles));

            if (Debug)
                builder.AppendSwitch("-Debug");

            return builder.ToString();
        }

        protected override string GenerateFullPathToTool()
        {
            if (AssetManCliPath.EndsWith("dll"))
                return "dotnet";

            return AssetManCliPath;
        }

        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            base.LogEventsFromTextOutput(singleLine, MessageImportance.Normal);

            var match = _outputRegex.Match(singleLine);
            if (match.Success)
            {
                var generatedAsset = match.Groups["generatedAsset"]
                                          .Value;
                var sourceAsset = match.Groups["sourceAsset"]
                                       .Value;
                if (File.Exists(generatedAsset) && File.Exists(sourceAsset))
                {
                    _generatedAssets.Add(generatedAsset);
                    _sourceAssets.Add(sourceAsset);
                }
            }
        }
    }
}
