/*
 * ResourceFolderManager.cs - Developed by Dan Wager for AndroidLib.dll - 04/12/12
 */

using System.Collections.Generic;
using System.IO;

namespace AndroidLib.Classes.Util
{
    /// <summary>
    ///     Controls Resource Folders for the <see cref="RegawMOD" /> Namespace
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         You can use this in your own programs to have a managed resource folder to extract your own files/write data
    ///         to.
    ///     </para>
    ///     <para>
    ///         Calling Unregister() deletes the directory and everything recursively in it, and removes unregisters the
    ///         directory in the manager.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <para>
    ///         This example shows how to register a new directory with the <see cref="ResourceFolderManager" />, and create
    ///         a .txt file in the folder.
    ///     </para>
    ///     <code>//This example shows how to register a new directory named "Testing" and create a file in the folder named "Test1.txt".
    /// 
    /// using System;
    /// using System.IO;
    /// using RegawMOD;
    /// 
    /// class Program
    /// {
    ///     static void Main(string[] args)
    ///     {
    ///         // Registers "Testing" with the manager
    ///         ResourceFolderManager.Register("Testing");
    ///         
    ///         // Creates file in the "Testing" folder
    ///         using (StreamWriter w = new StreamWriter(ResourceFolderManager.GetRegisteredFolderPath("Testing") + "Test1.txt"))
    ///         {
    ///             w.WriteLine("This is a test file about to be deleted by ResourceFolderManager.Unregister(\"Testing\")");
    ///         }
    ///         
    ///         // Removes folder from memory and file system, including all contents
    ///         ResourceFolderManager.Unregister("Testing");
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class ResourceFolderManager
    {
        private static readonly DirectoryInfo RegawmodTempDirectory;
        private static readonly Dictionary<string, DirectoryInfo> ControlledFolders;

        static ResourceFolderManager()
        {
            RegawmodTempDirectory = new DirectoryInfo(Path.GetTempPath() + "\\RegawMOD\\");
            ControlledFolders = new Dictionary<string, DirectoryInfo>();

            if (!RegawmodTempDirectory.Exists)
                RegawmodTempDirectory.Create();

            foreach (var d in RegawmodTempDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly))
                ControlledFolders.Add(d.Name, d);
        }

        /// <summary>
        ///     Gets a <see cref="DirectoryInfo" /> containing information about the registered resource directory
        ///     <paramref name="folder" />
        /// </summary>
        /// <param name="folder">Name of registered resource directory</param>
        /// <returns>
        ///     <see cref="DirectoryInfo" /> containing information about the registered resource directory
        ///     <paramref name="folder" />
        /// </returns>
        public static DirectoryInfo GetRegisteredFolder(string folder)
        {
            return ControlledFolders.ContainsKey(folder) ? ControlledFolders[folder] : null;
        }

        /// <summary>
        ///     Gets the full path of the registered resource directory <paramref name="folder" />
        /// </summary>
        /// <param name="folder">Name of registered resource directory</param>
        /// <returns>Full path of the registered resource directory <paramref name="folder" /></returns>
        public static string GetRegisteredFolderPath(string folder)
        {
            return ControlledFolders.ContainsKey(folder) ? ControlledFolders[folder].FullName : null;
        }

        /// <summary>
        ///     Registers and creates a temporary resource directory named <paramref name="name" /> with the
        ///     <see cref="ResourceFolderManager" />
        /// </summary>
        /// <param name="name">Name to give to resource directory</param>
        /// <returns>True if creation succeeds, false if directory already exists</returns>
        public static bool Register(string name)
        {
            if (ControlledFolders.ContainsKey(name))
                return false;

            ControlledFolders.Add(name, new DirectoryInfo(RegawmodTempDirectory + name));

            if (!ControlledFolders[name].Exists)
                ControlledFolders[name].Create();

            return true;
        }

        /// <summary>
        ///     Unregisters and removes the temporary resource directory defined in <paramref name="name" /> recursively
        /// </summary>
        /// <param name="name">Name of resource directory to unregister</param>
        /// <returns>True if deletion succeeds, false if not</returns>
        /// <remarks>
        ///     Make sure all resources in <paramref name="name" /> are not being used by the system at time of Unregister()
        ///     or it will return false.
        /// </remarks>
        public static bool Unregister(string name)
        {
            if (!ControlledFolders.ContainsKey(name))
                return false;

            try
            {
                ControlledFolders[name].Delete(true);
            }
            catch
            {
                return false;
            }

            return ControlledFolders.Remove(name);
        }
    }
}