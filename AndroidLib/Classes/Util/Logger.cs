using System;
using System.IO;

namespace AndroidLib.Classes.Util
{
    internal static class Logger
    {
        private static readonly string ErrorLogPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Path.Combine("AndroidLib", "ErrorLog.txt"));

        internal static bool WriteLog(string message, string title, string stackTrace)
        {
            try
            {
                using (
                    var fs = new FileStream(ErrorLogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                using (var sw = new StreamWriter(fs))
                    sw.WriteLine(string.Join(" ", title, message, stackTrace));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}