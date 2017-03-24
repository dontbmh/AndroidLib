/*
 * BusyBox.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System.Collections.Generic;
using System.IO;

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Conatins information about device's busybox
    /// </summary>
    public class BusyBox
    {
        internal const string Executable = "busybox";

        private readonly Device _device;

        internal BusyBox(Device device)
        {
            _device = device;

            Commands = new List<string>();

            Update();
        }

        /// <summary>
        ///     Gets a value indicating if busybox is installed on the current device
        /// </summary>
        public bool IsInstalled { get; private set; }

        /// <summary>
        ///     Gets a value indicating the version of busybox installed
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        ///     Gets a <c>List&lt;string&gt;</c> containing busybox's commands
        /// </summary>
        public List<string> Commands { get; }

        /// <summary>
        ///     Updates the instance of busybox
        /// </summary>
        /// <remarks>Generally called only if busybox may have changed on the device</remarks>
        public void Update()
        {
            Commands.Clear();

            if (!_device.HasRoot || _device.State != DeviceState.Online)
            {
                SetNoBusybox();
                return;
            }

            var adbCmd = Adb.FormAdbShellCommand(_device, false, Executable);
            using (var s = new StringReader(Adb.ExecuteAdbCommand(adbCmd)))
            {
                var check = s.ReadLine();

                if (check.Contains(string.Format("{0}: not found", Executable)))
                {
                    SetNoBusybox();
                    return;
                }

                IsInstalled = true;

                Version = check.Split(' ')[1].Substring(1);

                while (s.Peek() != -1 && s.ReadLine() != "Currently defined functions:")
                {
                }

                var cmds = s.ReadToEnd().Replace(" ", "").Replace("\r\r\n\t", "").Trim('\t', '\r', '\n').Split(',');

                if (cmds.Length.Equals(0))
                {
                    SetNoBusybox();
                }
                else
                {
                    foreach (var cmd in cmds)
                        Commands.Add(cmd);
                }
            }
        }

        private void SetNoBusybox()
        {
            IsInstalled = false;
            Version = null;
        }
    }
}