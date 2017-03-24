/*
 * Enums.cs - Developed by Dan Wager for AndroidLib.dll
 */

namespace AndroidLib.Classes.AndroidController
{
    /// <summary>
    ///     Specifies a FileSystem Listing
    /// </summary>
    public enum ListingType
    {
        /// <summary>
        ///     Represents a File
        /// </summary>
        File,

        /// <summary>
        ///     Represents a Directory
        /// </summary>
        Directory,

        /// <summary>
        ///     Represents neither File or Directory
        /// </summary>
        None,

        /// <summary>
        ///     Usually returned if BusyBox is not installed on device
        /// </summary>
        Error
    }

    /// <summary>
    ///     Specifies current state of <see cref="Device" />
    /// </summary>
    public enum DeviceState
    {
        /// <summary>
        ///     <see cref="Device" /> is online
        /// </summary>
        Online,

        /// <summary>
        ///     <see cref="Device" /> is offline
        /// </summary>
        Offline,

        /// <summary>
        ///     <see cref="Device" /> is in recovery
        /// </summary>
        Recovery,

        /// <summary>
        ///     <see cref="Device" /> is in fastboot
        /// </summary>
        Fastboot,

        /// <summary>
        ///     <see cref="Device" /> is in sideload mode
        /// </summary>
        Sideload,

        /// <summary>
        ///     <see cref="Device" /> is not authorized
        /// </summary>
        Unauthorized,

        /// <summary>
        ///     <see cref="Device" /> is in an unknown state
        /// </summary>
        Unknown
    }

    //public enum WifiEncryption
    //{
    //    WEP,
    //    WPA,
    //    WPA2
    //}

    /// <summary>
    ///     Specifies how to remount the file system
    /// </summary>
    public enum MountType
    {
        /// <summary>
        ///     Read-Writable
        /// </summary>
        Rw,

        /// <summary>
        ///     Read-Only
        /// </summary>
        Ro,

        /// <summary>
        ///     Used for every <see cref="DeviceState" /> except <see cref="DeviceState.Online" />
        /// </summary>
        None
    }

    /// <summary>
    ///     Specifies a certain partition of the connected Android device
    /// </summary>
    public enum DevicePartition
    {
        /// <summary>
        ///     The boot partition
        /// </summary>
        Boot,

        /// <summary>
        ///     The system partition
        /// </summary>
        System,

        /// <summary>
        ///     The data partition
        /// </summary>
        Data,

        /// <summary>
        ///     The hboot partition (bootloader)
        /// </summary>
        Hboot,

        /// <summary>
        ///     For flashing a zip
        /// </summary>
        Zip
    }

    /// <summary>
    ///     Specifies the keyevent code to send to "adb shell input keyevent {KeyEventCode}"
    /// </summary>
    /// <remarks>No root needed</remarks>
    public enum KeyEventCode
    {
        /// <summary>
        ///     The Soft Right Key
        /// </summary>
        SoftRight = 2,

        /// <summary>
        ///     The Home Button
        /// </summary>
        Home = 3,

        /// <summary>
        ///     The Back Button
        /// </summary>
        Back = 4,

        /// <summary>
        ///     The Call Button
        /// </summary>
        Call = 5,

        /// <summary>
        ///     The End Call Button
        /// </summary>
        EndCall = 6,

        /// <summary>
        ///     The Number 0 Button
        /// </summary>
        Number0 = 7,

        /// <summary>
        ///     The Number 1 Button
        /// </summary>
        Number1 = 8,

        /// <summary>
        ///     The Number 2 Button
        /// </summary>
        Number2 = 9,

        /// <summary>
        ///     The Number 3 Button
        /// </summary>
        Number3 = 10,

        /// <summary>
        ///     The Number 4 Button
        /// </summary>
        Number4 = 11,

        /// <summary>
        ///     The Number 5 Button
        /// </summary>
        Number5 = 12,

        /// <summary>
        ///     The Number 6 Button
        /// </summary>
        Number6 = 13,

        /// <summary>
        ///     The Number 7 Button
        /// </summary>
        Number7 = 14,

        /// <summary>
        ///     The Number 8 Button
        /// </summary>
        Number8 = 15,

        /// <summary>
        ///     The Number 9 Button
        /// </summary>
        Number9 = 16,

        /// <summary>
        ///     The Star Character (*) Button
        /// </summary>
        Star = 17,

        /// <summary>
        ///     The Pound Character (#) Button
        /// </summary>
        Pound = 18,

        /// <summary>
        ///     The D-Pad Up Button
        /// </summary>
        DpadUp = 19,

        /// <summary>
        ///     The D-Pad Down Button
        /// </summary>
        DpadDown = 20,

        /// <summary>
        ///     The D-Pad Left Button
        /// </summary>
        DpadLeft = 21,

        /// <summary>
        ///     The D-Pad Right Button
        /// </summary>
        DpadRight = 22,

        /// <summary>
        ///     The D-Pad Center Button
        /// </summary>
        DpadCenter = 23,

        /// <summary>
        ///     The Volume Up Button
        /// </summary>
        VolumeUp = 24,

        /// <summary>
        ///     The Volume Down Button
        /// </summary>
        VolumeDown = 25,

        /// <summary>
        ///     The Power Button
        /// </summary>
        Power = 26,

        /// <summary>
        ///     The Camera Button
        /// </summary>
        Camera = 27,

        /// <summary>
        ///     The Clear Button
        /// </summary>
        Clear = 28,

        /// <summary>
        ///     The A Character Button
        /// </summary>
        A = 29,

        /// <summary>
        ///     The B Character Button
        /// </summary>
        B = 30,

        /// <summary>
        ///     The C Character Button
        /// </summary>
        C = 31,

        /// <summary>
        ///     The D Character Button
        /// </summary>
        D = 32,

        /// <summary>
        ///     The E Character Button
        /// </summary>
        E = 33,

        /// <summary>
        ///     The F Character Button
        /// </summary>
        F = 34,

        /// <summary>
        ///     The G Character Button
        /// </summary>
        G = 35,

        /// <summary>
        ///     The H Character Button
        /// </summary>
        H = 36,

        /// <summary>
        ///     The I Character Button
        /// </summary>
        I = 37,

        /// <summary>
        ///     The J Character Button
        /// </summary>
        J = 38,

        /// <summary>
        ///     The K Character Button
        /// </summary>
        K = 39,

        /// <summary>
        ///     The L Character Button
        /// </summary>
        L = 40,

        /// <summary>
        ///     The M Character Button
        /// </summary>
        M = 41,

        /// <summary>
        ///     The N Character Button
        /// </summary>
        N = 42,

        /// <summary>
        ///     The O Character Button
        /// </summary>
        O = 43,

        /// <summary>
        ///     The P Character Button
        /// </summary>
        P = 44,

        /// <summary>
        ///     The Q Character Button
        /// </summary>
        Q = 45,

        /// <summary>
        ///     The R Character Button
        /// </summary>
        R = 46,

        /// <summary>
        ///     The S Character Button
        /// </summary>
        S = 47,

        /// <summary>
        ///     The T Character Button
        /// </summary>
        T = 48,

        /// <summary>
        ///     The U Character Button
        /// </summary>
        U = 49,

        /// <summary>
        ///     The V Character Button
        /// </summary>
        V = 50,

        /// <summary>
        ///     The W Character Button
        /// </summary>
        W = 51,

        /// <summary>
        ///     The X Character Button
        /// </summary>
        X = 52,

        /// <summary>
        ///     The Y Character Button
        /// </summary>
        Y = 53,

        /// <summary>
        ///     The Z Character Button
        /// </summary>
        Z = 54,

        /// <summary>
        ///     The Comma Character (,) Button
        /// </summary>
        Comma = 55,

        /// <summary>
        ///     The Period Character (.) Button
        /// </summary>
        Period = 56,

        /// <summary>
        ///     The Left Alt Button
        /// </summary>
        AltLeft = 57,

        /// <summary>
        ///     The Right Alt Button
        /// </summary>
        AltRight = 58,

        /// <summary>
        ///     The Left Shift Button
        /// </summary>
        ShiftLeft = 59,

        /// <summary>
        ///     The Right Shift Button
        /// </summary>
        ShiftRight = 60,

        /// <summary>
        ///     The Tab Button
        /// </summary>
        Tab = 61,

        /// <summary>
        ///     The Space Bar Button
        /// </summary>
        Space = 62,

        /// <summary>
        ///     Brings Up Select Input Method Dialog
        /// </summary>
        SelectInputMethod = 63,

        /// <summary>
        ///     Not Sure
        /// </summary>
        Explorer = 64,

        /// <summary>
        ///     Not Sure
        /// </summary>
        Envelope = 65,

        /// <summary>
        ///     The Enter Button
        /// </summary>
        Enter = 66,

        /// <summary>
        ///     The Delete Button
        /// </summary>
        Delete = 67,

        /// <summary>
        ///     Not Sure
        /// </summary>
        Grave = 68,

        /// <summary>
        ///     The Minus Button
        /// </summary>
        Minus = 69,

        /// <summary>
        ///     The Equals Character (=) Button
        /// </summary>
        EQUALS = 70,

        /// <summary>
        ///     The Left Bracket Character ({) Button
        /// </summary>
        BracketLeft = 71,

        /// <summary>
        ///     The Right Bracket Character (}) Button
        /// </summary>
        BracketRight = 72,

        /// <summary>
        ///     The Backslash Character (\) Button
        /// </summary>
        Backslash = 73,

        /// <summary>
        ///     The Semicolon Character (;) Button
        /// </summary>
        Semicolon = 74,

        /// <summary>
        ///     The Apostrophe Character (') Button
        /// </summary>
        Apostrophe = 75,

        /// <summary>
        ///     The Forward Slash Character (/) Button
        /// </summary>
        ForwardSlash = 76,

        /// <summary>
        ///     The At Character (@) Button
        /// </summary>
        At = 77,

        /// <summary>
        ///     Number Lock
        /// </summary>
        Num = 78,

        /// <summary>
        ///     Not Sure
        /// </summary>
        HeadsetHook = 79,

        /// <summary>
        ///     The Focus Camera Button
        /// </summary>
        Focus = 80,

        /// <summary>
        ///     The Plus Character (+) Button
        /// </summary>
        Plus = 81,

        /// <summary>
        ///     The Menu Button
        /// </summary>
        Menu = 82,

        /// <summary>
        ///     Not Sure
        /// </summary>
        Notification = 83,

        /// <summary>
        ///     The Search Button
        /// </summary>
        Search = 84,

        /// <summary>
        ///     Not Sure
        /// </summary>
        TagLastKeycode = 85
    }
}