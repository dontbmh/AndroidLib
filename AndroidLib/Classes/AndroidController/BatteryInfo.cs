/*
 * Battery.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System.IO;

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Contains information about connected Android device's battery
    /// </summary>
    public class BatteryInfo
    {
        private readonly Device _device;
        private bool _acPower;
        private string _dump;
        private int _health;
        private int _level;

        private string _outString;
        private bool _present;
        private int _scale;
        private int _status;
        private string _technology;
        private int _temperature;
        private bool _usbPower;
        private int _voltage;
        private bool _wirelessPower;

        /// <summary>
        ///     Initializes a new instance of the BatteryInfo class
        /// </summary>
        /// <param name="device">Serial number of Android device</param>
        internal BatteryInfo(Device device)
        {
            _device = device;
            Update();
        }

        /// <summary>
        ///     Gets a value indicating if the connected Android device is on AC Power
        /// </summary>
        public bool AcPower
        {
            get
            {
                Update();
                return _acPower;
            }
        }

        /// <summary>
        ///     Gets a value indicating if the connected Android device is on USB Power
        /// </summary>
        public bool UsbPower
        {
            get
            {
                Update();
                return _usbPower;
            }
        }

        /// <summary>
        ///     Gets a value indicating if the connected Android device is on Wireless Power
        /// </summary>
        public bool WirelessPower
        {
            get
            {
                Update();
                return _wirelessPower;
            }
        }

        /// <summary>
        ///     Gets a value indicating the status of the battery
        /// </summary>
        public string Status
        {
            /* As defined in: http://developer.android.com/reference/android/os/BatteryManager.html
             * Property "Status" is changed from type "int" to type "string" to give a string representation
             * of the value obtained from dumpsys regarding battery status.
             * Submitted By: Omar Bizreh [DeepUnknown from Xda-Developers.com]
             */
            get
            {
                Update();
                switch (_status)
                {
                    case 1:
                        return "Unknown Battery Status: " + _status;
                    case 2:
                        return "Charging";
                    case 3:
                        return "Discharging";
                    case 4:
                        return "Not charging";
                    case 5:
                        return "Full";
                    default:
                        return "Unknown Value: " + _status;
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating the health of the battery
        /// </summary>
        public string Health
        {
            /* As defined in: http://developer.android.com/reference/android/os/BatteryManager.html
             * Property "Health" is changed from type "int" to type "string" to give a string representation
             * of the value obtained from dumpsys regarding battery health.
             * Submitted By: Omar Bizreh [DeepUnknown from Xda-Developers.com]
             */
            get
            {
                Update();
                switch (_health)
                {
                    case 1:
                        return "Unknown Health State: " + _health;
                    case 2:
                        return "Good";
                    case 3:
                        return "Over Heat";
                    case 4:
                        return "Dead";
                    case 5:
                        return "Over Voltage";
                    case 6:
                        return "Unknown Failure";
                    case 7:
                        return "Cold Battery";
                    default:
                        return "Unknown Value: " + _health;
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating if there is a battery present
        /// </summary>
        public bool Present
        {
            get
            {
                Update();
                return _present;
            }
        }

        /// <summary>
        ///     Gets a value indicating the current charge level of the battery
        /// </summary>
        public int Level
        {
            get
            {
                Update();
                return _level;
            }
        }

        /// <summary>
        ///     Gets a value indicating the scale of the battery
        /// </summary>
        public int Scale
        {
            get
            {
                Update();
                return _scale;
            }
        }

        /// <summary>
        ///     Gets a value indicating the current voltage of the battery
        /// </summary>
        public int Voltage
        {
            get
            {
                Update();
                return _voltage;
            }
        }

        /// <summary>
        ///     Gets a value indicating the current temperature of the battery
        /// </summary>
        public int Temperature
        {
            get
            {
                Update();
                return _temperature;
            }
        }

        /// <summary>
        ///     Gets a value indicating the battery's technology
        /// </summary>
        public string Technology
        {
            get
            {
                Update();
                return _technology;
            }
        }

        private void Update()
        {
            if (_device.State != DeviceState.Online)
            {
                _acPower = false;
                _dump = null;
                _health = -1;
                _level = -1;
                _present = false;
                _scale = -1;
                _status = -1;
                _technology = null;
                _temperature = -1;
                _usbPower = false;
                _voltage = -1;
                _wirelessPower = false;
                _outString = "Device Not Online";
                return;
            }

            var adbCmd = Adb.FormAdbShellCommand(_device, false, "dumpsys", "battery");
            _dump = Adb.ExecuteAdbCommand(adbCmd);

            using (var r = new StringReader(_dump))
            {
                string line;

                while (true)
                {
                    line = r.ReadLine();

                    if (!line.Contains("Current Battery Service state"))
                    {
                    }
                    else
                    {
                        _dump = line + r.ReadToEnd();
                        break;
                    }
                }
            }

            using (var r = new StringReader(_dump))
            {
                var line = "";

                while (r.Peek() != -1)
                {
                    line = r.ReadLine();

                    if (line == "")
                        continue;
                    if (line.Contains("AC "))
                        bool.TryParse(line.Substring(14), out _acPower);
                    else if (line.Contains("USB"))
                        bool.TryParse(line.Substring(15), out _usbPower);
                    else if (line.Contains("Wireless"))
                        bool.TryParse(line.Substring(20), out _wirelessPower);
                    else if (line.Contains("status"))
                        int.TryParse(line.Substring(10), out _status);
                    else if (line.Contains("health"))
                        int.TryParse(line.Substring(10), out _health);
                    else if (line.Contains("present"))
                        bool.TryParse(line.Substring(11), out _present);
                    else if (line.Contains("level"))
                        int.TryParse(line.Substring(9), out _level);
                    else if (line.Contains("scale"))
                        int.TryParse(line.Substring(9), out _scale);
                    else if (line.Contains("voltage"))
                        int.TryParse(line.Substring(10), out _voltage);
                    else if (line.Contains("temp"))
                        int.TryParse(line.Substring(15), out _temperature);
                    else if (line.Contains("tech"))
                        _technology = line.Substring(14);
                }
            }

            _outString = _dump.Replace("Service state", "State For Device " + _device.SerialNumber);
        }

        /// <summary>
        ///     Returns a formatted string object containing all battery stats
        /// </summary>
        /// <returns>A formatted string containing all battery stats</returns>
        public override string ToString()
        {
            Update();
            return _outString;
        }
    }
}