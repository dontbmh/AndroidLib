/*
 * BuildProp.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System;
using System.Collections.Generic;
using AndroidLib.Classes.Util;

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Manages all information from connected Android device's build properties
    /// </summary>
    public class BuildProp
    {
        private readonly Device _device;

        private readonly Dictionary<string, string> _prop;

        internal BuildProp(Device device)
        {
            _prop = new Dictionary<string, string>();
            _device = device;
        }

        /// <summary>
        ///     Gets a <c>List&lt;string&gt;</c> containing all of the device's build proprty keys
        /// </summary>
        public List<string> Keys
        {
            get
            {
                Update();

                var keys = new List<string>();

                foreach (var key in _prop.Keys)
                    keys.Add(key);

                return keys;
            }
        }

        /// <summary>
        ///     Gets a <c>List&lt;string&gt;</c> object containing all of the device's build proprty values
        /// </summary>
        public List<string> Values
        {
            get
            {
                Update();

                var values = new List<string>();

                foreach (var val in _prop.Values)
                    values.Add(val);

                return values;
            }
        }

        /// <summary>
        ///     Gets the value of the specified build property key.
        /// </summary>
        /// <param name="key">Key of build property</param>
        /// <returns>Value if key exists, null if key doesn't exist</returns>
        public string GetProp(string key)
        {
            Update();

            string tmp;

            _prop.TryGetValue(key, out tmp);

            return tmp;
        }

        /// <summary>
        ///     Sets a build property value
        /// </summary>
        /// <remarks>
        ///     If <paramref name="key" /> does not exist or device does not have root, returns false, and does not set any
        ///     values
        /// </remarks>
        /// <param name="key">Build property key to set</param>
        /// <param name="newValue">Value you wish to set <paramref name="key" /> to</param>
        /// <returns>True if new value set, false if not</returns>
        public bool SetProp(string key, string newValue)
        {
            string before;
            if (!_prop.TryGetValue(key, out before))
                return false;

            if (!_device.HasRoot)
                return false;

            var adbCmd = Adb.FormAdbShellCommand(_device, true, "setprop", key, newValue);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);

            Update();

            string after;
            if (!_prop.TryGetValue(key, out after))
                return false;

            return newValue == after;
        }

        /// <summary>
        ///     Returns a formatted string containing all of the build properties
        /// </summary>
        /// <returns>Formatted string containing build.prop</returns>
        public override string ToString()
        {
            Update();

            var outPut = "";

            foreach (var s in _prop)
                outPut += string.Format("[{0}]: [{1}]" + Environment.NewLine, s.Key, s.Value);

            return outPut;
        }

        private void Update()
        {
            try
            {
                _prop.Clear();

                if (_device.State != DeviceState.Online)
                    return;

                var adbCmd = Adb.FormAdbShellCommand(_device, false, "getprop");
                var prop = Adb.ExecuteAdbCommand(adbCmd);

                var lines = prop.Split(new[] {"\r\n\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < lines.Length; i++)
                {
                    var entry = lines[i].Split(new[] {"[", "]: [", "]"}, StringSplitOptions.RemoveEmptyEntries);

                    if (entry.Length == 2)
                        _prop.Add(entry[0], entry[1]);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "Using: getprop in BuildProp.cs", ex.StackTrace);
            }
        }
    }
}