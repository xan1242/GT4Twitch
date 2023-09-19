using gt4pcsx2launcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GT4Twitch
{
    public class Win32
    {
        public static IntPtr GetFirstProcMod(IntPtr procHandle)
        {
            var mods = new IntPtr[16];

            Thread.Sleep(100); // HACK HACK HACK

            if (win32process.EnumProcessModules(procHandle, mods, (uint)Marshal.SizeOf<IntPtr>(), out _) == 0)
                return IntPtr.Zero;

            return mods[0];
        }

        public static bool WriteInt32(IntPtr handle, long addr, Int32 val)
        {
            byte[] str = BitConverter.GetBytes(val);
            IntPtr numbyteswritten = IntPtr.Zero;
            bool status = win32process.WriteProcessMemory(handle, (IntPtr)addr, str, sizeof(Int32), out numbyteswritten);

            return status;
        }

        public static bool WriteUInt32(IntPtr handle, long addr, UInt32 val)
        {
            byte[] str = BitConverter.GetBytes(val);
            IntPtr numbyteswritten = IntPtr.Zero;
            bool status = win32process.WriteProcessMemory(handle, (IntPtr)addr, str, sizeof(UInt32), out numbyteswritten);

            return status;
        }

        public static bool ReadInt32(IntPtr handle, long addr, out Int32 val)
        {
            var str = new byte[32];
            IntPtr numbytesread = IntPtr.Zero;
            bool status = win32process.ReadProcessMemory(handle, (IntPtr)addr, str, sizeof(Int32), out numbytesread);

            val = 0;

            if (status == true)
            {
                val = (int)Marshal.ReadInt32(str, 0);
            }

            return status;
        }

        public static bool ReadUInt32(IntPtr handle, long addr, out UInt32 val)
        {
            var str = new byte[32];
            IntPtr numbytesread = IntPtr.Zero;
            bool status = win32process.ReadProcessMemory(handle, (IntPtr)addr, str, sizeof(UInt32), out numbytesread);

            val = 0;

            if (status == true)
            {
                val = (uint)Marshal.ReadInt32(str, 0);
            }

            return status;
        }
    }
}
