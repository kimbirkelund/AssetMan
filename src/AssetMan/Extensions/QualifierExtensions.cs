using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace AssetMan.Extensions
{
    public static class QualifierExtensions
    {
        public static (string name, string extension) WithoutQualifier([NotNull] this string path)
        {
            path = path ?? throw new ArgumentNullException(nameof(path));

            var name = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Parameter contains no file name.", nameof(path));

            var extension = Path.GetExtension(path);

            var splits = name.Split('@');
            if (splits.Length > 1)
                name = string.Join("@", splits.Take(splits.Length - 1));

            return (name, extension);
        }
    }
}
