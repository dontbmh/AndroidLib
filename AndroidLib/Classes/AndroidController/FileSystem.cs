/*
 * FileSystem.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Contains mount directory information
    /// </summary>
    public class MountInfo
    {
        internal MountInfo(string directory, string block, MountType type)
        {
            Directory = directory;
            Block = block;
            MountType = type;
        }

        /// <summary>
        ///     Gets a value indicating the mount directory
        /// </summary>
        public string Directory { get; }

        /// <summary>
        ///     Gets a value indicating the mount block
        /// </summary>
        public string Block { get; }

        /// <summary>
        ///     Gets a value indicating how the mount directory is mounted
        /// </summary>
        /// <remarks>See <see cref="MountType" /> for more details</remarks>
        public MountType MountType { get; }
    }

    /// <summary>
    ///     Contains information about the Android device's file system
    /// </summary>
    public class FileSystem
    {
        private const string IsFile = "if [ -f {0} ]; then echo \"1\"; else echo \"0\"; fi";
        private const string IsDirectory = "if [ -d {0} ]; then echo \"1\"; else echo \"0\"; fi";
        private readonly Device _device;

        private MountInfo _systemMount;

        internal FileSystem(Device device)
        {
            _device = device;
            UpdateMountPoints();
        }

        /// <summary>
        ///     Gets the <see cref="MountInfo" /> containing information about the /system mount directory
        /// </summary>
        /// <remarks>See <see cref="MountInfo" /> for more details</remarks>
        public MountInfo SystemMountInfo
        {
            get
            {
                UpdateMountPoints();
                return _systemMount;
            }
        }

        private void UpdateMountPoints()
        {
            if (_device.State != DeviceState.Online)
            {
                _systemMount = new MountInfo(null, null, MountType.None);
                return;
            }

            var adbCmd = Adb.FormAdbShellCommand(_device, false, "mount");
            using (var r = new StringReader(Adb.ExecuteAdbCommand(adbCmd)))
            {
                while (r.Peek() != -1)
                {
                    var line = r.ReadLine();
                    var splitLine = line.Split(' ');

                    string dir;
                    string mount;
                    MountType type;
                    try
                    {
                        if (line.Contains(" on /system "))
                        {
                            dir = splitLine[2];
                            mount = splitLine[0];
                            type = (MountType) Enum.Parse(typeof (MountType), splitLine[5].Substring(1, 2).ToUpper());
                            _systemMount = new MountInfo(dir, mount, type);
                            return;
                        }

                        if (line.Contains(" /system "))
                        {
                            dir = splitLine[1];
                            mount = splitLine[0];
                            type = (MountType) Enum.Parse(typeof (MountType), splitLine[3].Substring(0, 2).ToUpper());
                            _systemMount = new MountInfo(dir, mount, type);
                            return;
                        }
                    }
                    catch
                    {
                        dir = "/system";
                        mount = "ERROR";
                        type = MountType.None;
                        _systemMount = new MountInfo(dir, mount, type);
                    }
                }
            }
        }

        //void PushFile();
        //void PullFile();

        /// <summary>
        ///     Mounts connected Android device's file system as specified
        /// </summary>
        /// <param name="type">The desired <see cref="MountType" /> (RW or RO)</param>
        /// <returns>True if remount is successful, False if remount is unsuccessful</returns>
        /// <example>
        ///     The following example shows how you can mount the file system as Read-Writable or Read-Only
        ///     <code>
        ///  // This example demonstrates mounting the Android device's file system as Read-Writable
        ///  using System;
        ///  using RegawMOD.Android;
        ///  
        ///  class Program
        ///  {
        ///      static void Main(string[] args)
        ///      {
        ///          AndroidController android = AndroidController.Instance;
        ///          Device device;
        ///          
        ///          Console.WriteLine("Waiting For Device...");
        ///          android.WaitForDevice(); //This will wait until a device is connected to the computer
        ///          device = android.ConnectedDevices[0]; //Sets device to the first Device in the collection
        /// 
        ///          Console.WriteLine("Connected Device - {0}", device.SerialNumber);
        /// 
        ///          Console.WriteLine("Mounting System as RW...");
        ///      	Console.WriteLine("Mount success? - {0}", device.RemountSystem(MountType.RW));
        ///      }
        ///  }
        ///  
        /// 	// The example displays the following output (if mounting is successful):
        /// 	//		Waiting For Device...
        /// 	//		Connected Device - {serial # here}
        /// 	//		Mounting System as RW...
        /// 	//		Mount success? - true
        ///  </code>
        /// </example>
        public bool RemountSystem(MountType type)
        {
            if (!_device.HasRoot)
                return false;

            var adbCmd = Adb.FormAdbShellCommand(_device, true, "mount",
                string.Format("-o remount,{0} -t yaffs2 {1} /system", type.ToString().ToLower(), _systemMount.Block));
            Adb.ExecuteAdbCommandNoReturn(adbCmd);

            UpdateMountPoints();

            if (_systemMount.MountType == type)
                return true;

            return false;
        }

        /// <summary>
        ///     Gets a <see cref="ListingType" /> indicating is the requested location is a File or Directory
        /// </summary>
        /// <param name="location">Path of requested location on device</param>
        /// <returns>See <see cref="ListingType" /></returns>
        /// <remarks>
        ///     <para>Requires a device containing BusyBox for now, returns ListingType.ERROR if not installed.</para>
        ///     <para>Returns ListingType.NONE if file/Directory does not exist</para>
        /// </remarks>
        public ListingType FileOrDirectory(string location)
        {
            if (!_device.BusyBox.IsInstalled)
                return ListingType.Error;

            var isFile = Adb.FormAdbShellCommand(_device, false, string.Format(IsFile, location));
            var isDir = Adb.FormAdbShellCommand(_device, false, string.Format(IsDirectory, location));

            if (Adb.ExecuteAdbCommand(isFile).Contains("1"))
                return ListingType.File;
            if (Adb.ExecuteAdbCommand(isDir).Contains("1"))
                return ListingType.Directory;

            return ListingType.None;
        }

        /// <summary>
        ///     Gets a <see cref="Dictionary
        ///     
        ///     <string, ListingType>"/> containing all the files and folders in the directory added as a parameter.
        /// </summary>
        /// <param name="rootDir">
        ///     The directory you'd like to list the files and folders from.
        ///     E.G.: /system/bin/
        /// </param>
        /// <returns>See <see cref="Dictionary" /></returns>
        public Dictionary<string, ListingType> GetFilesAndDirectories(string location)
        {
            if (string.IsNullOrEmpty(location) || Regex.IsMatch(location, @"\s"))
                throw new ArgumentException("rootDir must not be null or empty!");

            var filesAndDirs = new Dictionary<string, ListingType>();
            AdbCommand cmd;

            if (_device.BusyBox.IsInstalled)
                cmd = Adb.FormAdbShellCommand(_device, true, "busybox", "ls", "-a", "-p", "-l", location);
            else
                cmd = Adb.FormAdbShellCommand(_device, true, "ls", "-a", "-p", "-l", location);

            using (var reader = new StringReader(Adb.ExecuteAdbCommand(cmd)))
            {
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line) && !Regex.IsMatch(line, @"\s"))
                    {
                        filesAndDirs.Add(line, line.EndsWith("/") ? ListingType.Directory : ListingType.File);
                    }
                }
            }


            return filesAndDirs;
        }
    }
}