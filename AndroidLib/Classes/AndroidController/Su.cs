/*
 * Su.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System.IO;

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Contains information about the Su binary on the Android device
    /// </summary>
    public class Su
    {
        private readonly Device _device;

        internal Su(Device device)
        {
            _device = device;
            GetSuData();
        }

        internal bool Exists { get; private set; }

        /// <summary>
        ///     Gets a value indicating the version of Su on the Android device
        /// </summary>
        public string Version { get; private set; }

        private void GetSuData()
        {
            if (_device.State != DeviceState.Online)
            {
                Version = null;
                Exists = false;
                return;
            }

            var adbCmd = Adb.FormAdbShellCommand(_device, false, "su", "-v");
            using (var r = new StringReader(Adb.ExecuteAdbCommand(adbCmd)))
            {
                var line = r.ReadLine();

                if (line.Contains("not found") || line.Contains("permission denied"))
                {
                    Version = "-1";
                    Exists = false;
                }
                else
                {
                    Version = line;
                    Exists = true;
                }
            }
        }
    }
}