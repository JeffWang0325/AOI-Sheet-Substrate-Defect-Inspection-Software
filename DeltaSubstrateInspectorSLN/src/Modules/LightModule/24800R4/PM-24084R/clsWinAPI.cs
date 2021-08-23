using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace PM_24084R
{
    public sealed class clsWinAPI
    {
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static StringBuilder GetString = new StringBuilder(255);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(IntPtr classname, string title); // extern method: FindWindow

        [DllImport("user32.dll")]
        public static extern void MoveWindow(IntPtr hwnd, int X, int Y,
            int nWidth, int nHeight, bool rePaint); // extern method: MoveWindow

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect
            (IntPtr hwnd, out Rectangle rect); // extern method: GetWindowRect

    }
}
