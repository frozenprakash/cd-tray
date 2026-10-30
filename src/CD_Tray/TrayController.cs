using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CD_Tray
{
    /// <summary>Opens and closes the tray of an optical drive through the Windows MCI API.</summary>
    internal sealed class TrayController : IDisposable
    {
        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        private static extern int mciSendString(string command, StringBuilder returnString, int returnLength, IntPtr callback);

        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        private static extern bool mciGetErrorString(int error, StringBuilder buffer, int bufferLength);

        private readonly string _alias = "cdtray" + Guid.NewGuid().ToString("N");

        public TrayController(char driveLetter)
        {
            Send("open " + driveLetter + ": type CDAudio alias " + _alias);
        }

        public static char? FindFirstOpticalDrive()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom)
                    return drive.Name[0];
            }
            return null;
        }

        public void Open() => Send("set " + _alias + " door open");

        public void Close() => Send("set " + _alias + " door closed");

        public void Dispose() => mciSendString("close " + _alias, null, 0, IntPtr.Zero);

        private static void Send(string command)
        {
            int error = mciSendString(command, null, 0, IntPtr.Zero);
            if (error == 0)
                return;

            var message = new StringBuilder(256);
            if (!mciGetErrorString(error, message, message.Capacity))
                message.Append("MCI error " + error);
            throw new Win32Exception(error, message.ToString());
        }
    }
}
