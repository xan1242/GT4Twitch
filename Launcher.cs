using gt4pcsx2launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using win32structs;

namespace GT4Twitch
{
    internal class Launcher
    {
        public static string GetRootPath(string filePath)
        {
            int lastSeparatorIndex = filePath.LastIndexOfAny(new char[] { '\\', '/' });
            if (lastSeparatorIndex >= 0)
            {
                return filePath.Substring(0, lastSeparatorIndex);
            }
            return filePath; // If no separators found, return the same path
        }

        public static IntPtr OpenApp(string path, string working_dir)
        {
            STARTUPINFO info = new STARTUPINFO();
            info.cb = 104;
            PROCESS_INFORMATION process_info = new PROCESS_INFORMATION();

            bool rv = win32process.CreateProcess(null, path, IntPtr.Zero, IntPtr.Zero, false, 0x10 | 0x08000000, IntPtr.Zero, working_dir, ref info, out process_info);

            if (rv == false)
            {
                Console.WriteLine("CreateProcess fail\n");
                return IntPtr.Zero;
            }

            return process_info.hProcess;
        }
    }
}
