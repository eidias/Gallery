using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Service.Helpers
{
    //The class should be named like this according to CA1060
    public class NativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "AttachConsole")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AttachConsole(Int32 dwProcessId);

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();
    }
}
