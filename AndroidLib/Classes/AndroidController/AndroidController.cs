/*
 * AndroidController.cs - Handles communication between computer and Android devices
 * Developed by Dan Wager for AndroidLib.dll - 04/12/12
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AndroidLib.Classes.Util;

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Controls communication to and from connected Android devices.  Use only one instance for the entire project.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="AndroidController" /> is the core class in AndroidLib. You must always call the <c>Dispose()</c>
    ///         method when finished before program exits.
    ///     </para>
    ///     <para>
    ///         <see cref="AndroidController" /> specifically controls the Android Debug Bridge Server, and a developer
    ///         should NEVER try to start/kill the server using an <see cref="AdbCommand" />
    ///     </para>
    /// </remarks>
    /// <example>
    ///     The following example shows how you can use the <c>AndroidController</c> class
    ///     <code>
    ///  // This example demonstrates using AndroidController, and writing the first connected Android device's serial number to the console
    ///  using System;
    ///  using RegawMOD.Android;
    /// 
    ///  class Program
    ///  {
    ///      static void Main(string[] args)
    ///      {
    ///          AndroidController android = AndroidController.Instance;
    ///          Device device;
    ///          string serialNumber;
    ///          
    ///          Console.WriteLine("Waiting For Device...");
    /// 
    ///          // This will wait until a device is connected to the computer
    ///          // Should ONLY be used in Console applications though, as it freezes WinForm apps
    ///          android.WaitForDevice();
    /// 
    ///          // Gets first serial number of Device in collection
    ///          serialNumber = android.ConnectedDevices[0];
    /// 
    ///          // New way to set 'device' to the first Device in the collection
    ///          device = android.GetConnectedDevice(serialNumber);
    /// 
    ///          Console.WriteLine("Connected Device - {0}", device.SerialNumber);
    ///          
    ///          android.Dispose();
    ///      }
    ///  }
    ///  
    /// 	// The example displays the following output:
    /// 	//		Waiting For Device...
    /// 	//		Connected Device - {serial # here}
    ///  </code>
    /// </example>
    public sealed class AndroidController
    {
        private const string AndroidControllerTmpFolder = "AndroidLib\\";

        private static readonly Dictionary<string, string> Resources = new Dictionary<string, string>
        {
            {"adb.exe", "862c2b75b223e3e8aafeb20fe882a602"},
            {"AdbWinApi.dll", "47a6ee3f186b2c2f5057028906bac0c6"},
            {"AdbWinUsbApi.dll", "5f23f2f936bdfac90bb0a4970ad365cf"},
            {"fastboot.exe", "35792abb2cafdf2e6844b61e993056e2"}
        };

        private static AndroidController _instance;

        private readonly List<string> _connectedDevices;
        private bool _extractResources;

        private AndroidController()
        {
            _connectedDevices = new List<string>();
            ResourceFolderManager.Register(AndroidControllerTmpFolder);
            ResourceDirectory = ResourceFolderManager.GetRegisteredFolderPath(AndroidControllerTmpFolder);
        }

        /// <summary>
        ///     Gets the current AndroidController Instance.
        /// </summary>
        public static AndroidController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AndroidController();
                    _instance.CreateResourceDirectories();
                    _instance.ExtractResources();
                    Adb.StartServer();
                }

                return _instance;
            }
        }

        /// <summary>
        ///     Gets a <c>List&lt;string&gt;</c> object containing the serial numbers of all connected Android devices
        /// </summary>
        public List<string> ConnectedDevices
        {
            get
            {
                UpdateDeviceList();
                return _connectedDevices;
            }
        }

        internal string ResourceDirectory { get; }

        /// <summary>
        ///     Gets a value indicating if there are any Android devices currently connected
        /// </summary>
        public bool HasConnectedDevices
        {
            get
            {
                UpdateDeviceList();
                return _connectedDevices.Count > 0 ? true : false;
            }
        }

        /// <summary>
        ///     Set to true to cancel a WaitForDevice() method call
        /// </summary>
        public bool CancelWait { get; set; }

        private void CreateResourceDirectories()
        {
            try
            {
                if (!Adb.ExecuteAdbCommand(new AdbCommand("version")).Contains(Adb.AdbVersion))
                {
                    Adb.KillServer();
                    Thread.Sleep(1000);
                    ResourceFolderManager.Unregister(AndroidControllerTmpFolder);
                    _extractResources = true;
                }
            }
            catch (Exception)
            {
                _extractResources = true;
            }
            ResourceFolderManager.Register(AndroidControllerTmpFolder);
        }

        private void ExtractResources()
        {
            if (_extractResources)
            {
                var res = new string[Resources.Count];
                Resources.Keys.CopyTo(res, 0);
                Extract.Resources(this, ResourceDirectory, "Resources.AndroidController", res);
            }
        }

        /// <summary>
        ///     Releases all resources used by <see cref="AndroidController" />
        /// </summary>
        /// <remarks>Needs to be called when application has finished using <see cref="AndroidController" /></remarks>
        public void Dispose()
        {
            if (Adb.ServerRunning)
            {
                Adb.KillServer();
                Thread.Sleep(1000);
            }
            _instance = null;
        }

        /// <summary>
        ///     Gets the first <see cref="Device" /> in the internal collection of devices controlled by
        ///     <see cref="AndroidController" />
        /// </summary>
        /// <returns><see cref="Device" /> containing info about the device with the first serial number in the internal collection</returns>
        public Device GetConnectedDevice()
        {
            if (HasConnectedDevices)
                return new Device(_connectedDevices[0]);

            return null;
        }

        /// <summary>
        ///     Gets a <see cref="Device" /> containing data about a specified Android device.
        /// </summary>
        /// <remarks><paramref name="deviceSerial" /> must be a serial number of a connected device, or the method returns null</remarks>
        /// <param name="deviceSerial">Serial number of connected device</param>
        /// <returns>
        ///     <see cref="Device" /> containing info about the device with the serial number <paramref name="deviceSerial" />
        /// </returns>
        public Device GetConnectedDevice(string deviceSerial)
        {
            UpdateDeviceList();

            if (_connectedDevices.Contains(deviceSerial))
                return new Device(deviceSerial);

            return null;
        }

        /// <summary>
        ///     Determines if the Android device with the serial number provided is currently connected
        /// </summary>
        /// <example>
        ///     The following example shows how to use <c>IsDeviceConnected(string deviceSerial)</c> in one of your programs
        ///     <code>
        /// //This example demonstrates how to use IsDeviceConnected(string deviceSerial) in your project
        /// //This example assumes there is an instance of AndroidController running named android.
        /// 
        /// string serialNumber = "HTC123456789";
        /// 
        /// bool currentlyConnected = android.IsDeviceConnected(serialNumber);
        /// </code>
        /// </example>
        /// <param name="deviceSerial">Serial number of Android device</param>
        /// <returns>A value indicating if the Android device with the serial number <paramref name="deviceSerial" /> is connected</returns>
        public bool IsDeviceConnected(string deviceSerial)
        {
            UpdateDeviceList();

            foreach (var s in _connectedDevices)
                if (s.ToLower() == deviceSerial.ToLower())
                    return true;

            return false;
        }

        /// <summary>
        ///     Determines if the Android device tied to <paramref name="device" /> is currently connected
        /// </summary>
        /// <param name="device">Instance of <see cref="Device" /></param>
        /// <returns>A value indicating if the Android device indicated in <paramref name="device" /> is connected</returns>
        public bool IsDeviceConnected(Device device)
        {
            UpdateDeviceList();

            foreach (var d in _connectedDevices)
                if (d == device.SerialNumber)
                    return true;

            return false;
        }

        /// <summary>
        ///     Updates Internal Device List
        /// </summary>
        /// <remarks>Call this before checking for Devices, or setting a new Device, for most updated results</remarks>
        public void UpdateDeviceList()
        {
            var deviceList = "";

            _connectedDevices.Clear();

            deviceList = Adb.Devices();
            if (deviceList.Length > 29)
            {
                using (var s = new StringReader(deviceList))
                {
                    string line;

                    while (s.Peek() != -1)
                    {
                        line = s.ReadLine();

                        if (line.StartsWith("List") || line.StartsWith("\r\n") || line.Trim() == "")
                            continue;

                        if (line.IndexOf('\t') != -1)
                        {
                            line = line.Substring(0, line.IndexOf('\t'));
                            _connectedDevices.Add(line);
                        }
                    }
                }
            }

            deviceList = Fastboot.Devices();
            if (deviceList.Length > 0)
            {
                using (var s = new StringReader(deviceList))
                {
                    string line;

                    while (s.Peek() != -1)
                    {
                        line = s.ReadLine();

                        if (line.StartsWith("List") || line.StartsWith("\r\n") || line.Trim() == "")
                            continue;

                        if (line.IndexOf('\t') != -1)
                        {
                            line = line.Substring(0, line.IndexOf('\t'));
                            _connectedDevices.Add(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Pauses thread until 1 or more Android devices are connected
        /// </summary>
        /// <remarks>
        ///     Do Not Use in Windows Forms applications, as this method pauses the current thread.  Works fine in Console
        ///     Applications
        /// </remarks>
        public void WaitForDevice()
        {
            /* Entering an endless loop will exhaust CPU. 
             * Since this method must be called in a child thread in Windows Presentation Foundation (WPF) or Windows Form Apps,
             * sleeping thread for 250 miliSecond (1/4 of a second)
             * will be more friendly to the CPU. Nonetheless checking 4 times for a connected device in each second is more than enough,
             * and will not result in late response from the app if a device gets connected. 
             */
            while (!HasConnectedDevices && !CancelWait)
            {
                Thread.Sleep(250);
            }

            CancelWait = false;
        }
    }
}