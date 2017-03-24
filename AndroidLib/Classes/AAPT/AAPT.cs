using System;
using System.Collections.Generic;
using System.IO;
using AndroidLib.Classes.Util;

namespace AndroidLib.Classes.AAPT
{
    /// <summary>
    ///     Wrapper for the AAPT Android binary
    /// </summary>
    public partial class Aapt : IDisposable
    {
        private static readonly Dictionary<string, string> Resources = new Dictionary<string, string>
        {
            {"aapt.exe", "26a35ee028ed08d7ad0d18ffb6bb587a"}
        };

        private readonly string _resDir;

        /// <summary>
        ///     Initializes a new instance of the <c>AAPT</c> class
        /// </summary>
        public Aapt()
        {
            ResourceFolderManager.Register("AAPT");
            _resDir = ResourceFolderManager.GetRegisteredFolderPath("AAPT");

            ExtractResources(_resDir);
        }

        /// <summary>
        ///     Call to free up resources after use of <c>AAPT</c>
        /// </summary>
        public void Dispose()
        {
            ResourceFolderManager.Unregister("AAPT");
        }

        /// <summary>
        ///     Dumps the specified Apk's badging information
        /// </summary>
        /// <param name="source">Source Apk on local machine</param>
        /// <returns><see cref="Aapt.Badging" /> object containing badging information</returns>
        public Badging DumpBadging(FileInfo source)
        {
            if (!source.Exists)
                throw new FileNotFoundException();

            return new Badging(source,
                Command.RunProcessReturnOutput(Path.Combine(_resDir, "aapt.exe"),
                    "dump badging \"" + source.FullName + "\"", true, Command.DefaultTimeout));
        }

        private void ExtractResources(string path)
        {
            var res = new string[Resources.Count];
            Resources.Keys.CopyTo(res, 0);

            Extract.Resources("RegawMOD.Android", path, "Resources.AAPT", res);
        }
    }
}