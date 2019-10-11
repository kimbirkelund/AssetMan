using System;
using System.Diagnostics;

namespace AssetMan
{
    public static class Log
    {
        public static Action<string> Write { get; set; } = m => Debug.WriteLine(m);
    }
}
