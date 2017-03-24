/*
 * Phone.cs - Developed by Dan Wager for AndroidLib.dll
 */

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Controls radio options on an Android device
    /// </summary>
    public class Phone
    {
        private readonly Device _device;

        internal Phone(Device device)
        {
            _device = device;
        }

        /// <summary>
        ///     Calls a phone number on the Android device
        /// </summary>
        /// <param name="phoneNumber">Phone number to call</param>
        public void CallPhoneNumber(string phoneNumber)
        {
            if (_device.State != DeviceState.Online)
                return;

            var adbCmd = Adb.FormAdbShellCommand(_device, false, "service", "call", "phone", "2", "s16", phoneNumber);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);
            adbCmd = Adb.FormAdbShellCommand(_device, false, "input", "keyevent", (int) KeyEventCode.Back);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);
        }

        /// <summary>
        ///     Dials (does not call) a phone number on the Android device
        /// </summary>
        /// <param name="phoneNumber">Phone number to dial</param>
        public void DialPhoneNumber(string phoneNumber)
        {
            if (_device.State != DeviceState.Online)
                return;

            var adbCmd = Adb.FormAdbShellCommand(_device, false, "service", "call", "phone", "1", "s16", phoneNumber);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);
        }

        //public void SendSMS(string phoneNumber, string messageContents)
        //{
        //    throw new NotImplementedException();

        //    try { this.device.Processes.KillProcess(this.device.Processes["com.android.mms"]); }
        //    catch { }
        //    AdbCommand adbCmd = Adb.FormAdbShellCommand(this.device, false, "am", "start", "-a android.intent.action.SENDTO", "-d sms:" + phoneNumber, "--es sms_body \"" + messageContents + "\"", "--ez exit_on_sent true");
        //    adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.DPAD_RIGHT);
        //    adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.ENTER);
        //    adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.HOME);
        //}
    }
}