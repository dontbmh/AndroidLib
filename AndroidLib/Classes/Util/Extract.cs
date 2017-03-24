/*
 * Extract.cs - Extract Embedded Resources
 * Developed by Dan Wager - 06/22/2011
 */

using System.IO;
using System.Reflection;

namespace AndroidLib.Classes.Util
{
    internal static class Extract
    {
        /// <summary>
        ///     Extracts Multiple Embedded Resources From Calling Assembly
        /// </summary>
        /// <param name="obj">Object To Derive Default Namespace From</param>
        /// <param name="internalFolderPath">Period . Delimited path of embedded resources in assembly</param>
        /// <param name="fullPathOfItems">Exact Names Of Embedded Resources to Extract</param>
        /// <param name="outDirectory">Full Directory of Path For Extracted Resources</param>
        internal static void Resources(object obj, string outDirectory, string internalFolderPath,
            params string[] fullPathOfItems)
        {
            var assembly = Assembly.GetCallingAssembly();
            var defaultNamespace = obj.GetType().Namespace;

            foreach (var item in fullPathOfItems)
                using (
                    var s =
                        assembly.GetManifestResourceStream(defaultNamespace + "." +
                                                           (internalFolderPath == null ? "" : internalFolderPath + ".") +
                                                           item))
                using (var r = new BinaryReader(s))
                using (var fs = new FileStream(outDirectory + "\\" + item, FileMode.OpenOrCreate))
                using (var w = new BinaryWriter(fs))
                    w.Write(r.ReadBytes((int) s.Length));
        }

        /// <summary>
        ///     Extracts Multiple Embedded Resources From Calling Assembly
        /// </summary>
        /// <param name="nameSpace">Namespace of calling assembly</param>
        /// <param name="outDirectory">Full Directory of Path For Extracted Resources</param>
        /// <param name="internalFolderPath">Period . Delimited path of embedded resources in assembly</param>
        /// <param name="fullPathOfItems">Exact Names Of Embedded Resources to Extract</param>
        internal static void Resources(string nameSpace, string outDirectory, string internalFolderPath,
            params string[] fullPathOfItems)
        {
            var assembly = Assembly.GetCallingAssembly();

            foreach (var item in fullPathOfItems)
                using (
                    var s =
                        assembly.GetManifestResourceStream(nameSpace + "." +
                                                           (internalFolderPath == null ? "" : internalFolderPath + ".") +
                                                           item))
                using (var r = new BinaryReader(s))
                using (var fs = new FileStream(outDirectory + "\\" + item, FileMode.OpenOrCreate))
                using (var w = new BinaryWriter(fs))
                    w.Write(r.ReadBytes((int) s.Length));
        }
    }
}