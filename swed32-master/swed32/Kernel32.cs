﻿using System.Runtime.InteropServices;

namespace Swed32;

public static class Kernel32
{
    [DllImport("Kernel32.dll")]

    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [Out] byte[] lpBuffer,
        int nSize,
        IntPtr lpNumberOfBytesRead
    );


    [DllImport("kernel32.dll")]

    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int size,
        IntPtr lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile
    );

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);
}