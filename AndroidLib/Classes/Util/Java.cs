/*
 * Java.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System;
using System.IO;
using Microsoft.Win32;

namespace AndroidLib.Classes.Util
{
    /// <summary>
    ///     Contains information about the current machine's Java installation
    /// </summary>
    public static class Java
    {
        static Java()
        {
            Update();
        }

        /// <summary>
        ///     Gets a value indicating if Java is currently installed on the local machine
        /// </summary>
        public static bool IsInstalled { get; private set; }

        /// <summary>
        ///     Gets a value indicating the installation path of Java on the local machine
        /// </summary>
        public static string InstallationPath { get; private set; }

        /// <summary>
        ///     Gets a value indicating the path to Java's bin directory on the local machine
        /// </summary>
        public static string BinPath { get; private set; }

        /// <summary>
        ///     Gets a value indicating the path to Java.exe on the local machine
        /// </summary>
        public static string JavaExe { get; private set; }

        /// <summary>
        ///     Gets a value indicating the path to Javac.exe on the local machine
        /// </summary>
        public static string JavacExe { get; private set; }

        /// <summary>
        ///     Updates the information stored in the <see cref="Java" /> class
        /// </summary>
        /// <remarks>Generally called if Java installation might have changed</remarks>
        public static void Update()
        {
            InstallationPath = GetJavaInstallationPath();
            IsInstalled = !string.IsNullOrEmpty(InstallationPath);

            if (IsInstalled)
            {
                BinPath = Path.Combine(InstallationPath, "bin");
                JavaExe = Path.Combine(InstallationPath, "bin\\java.exe");
                JavacExe = Path.Combine(InstallationPath, "bin\\javac.exe");
            }
        }

        private static string GetJavaInstallationPath()
        {
            var environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");

            if (!string.IsNullOrEmpty(environmentPath))
                return environmentPath;

            var javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";

            try
            {
                using (var r = Registry.LocalMachine.OpenSubKey(javaKey))
                {
                    using (var k = r.OpenSubKey(r.GetValue("CurrentVersion").ToString()))
                    {
                        environmentPath = k.GetValue("JavaHome").ToString();
                    }
                }
            }
            catch
            {
                environmentPath = null;
            }

            return environmentPath;
        }

        /// <summary>
        ///     Runs the specified Jar file with the specified arguments
        /// </summary>
        /// <param name="pathToJar">Full path the Jar file on local machine</param>
        /// <param name="arguments">Arguments to pass to the Jar at runtime</param>
        /// <returns>True if successful run, false if Java is not installed or the Jar does not exist</returns>
        public static bool RunJar(string pathToJar, params string[] arguments)
        {
            if (!IsInstalled)
                return false;

            if (!File.Exists(pathToJar))
                return false;

            var args = "-jar " + pathToJar;

            for (var i = 0; i < arguments.Length; i++)
                args += " " + arguments[i];

            Command.RunProcessNoReturn(JavaExe, args, Command.DefaultTimeout);

            return true;
        }
    }
}