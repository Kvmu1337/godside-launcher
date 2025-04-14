using System;
using System.Runtime.InteropServices;

public static class NativeMethods
{
    // Importing the printAsciiArt function
    [DllImport("Dll1.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern void printAsciiArt();

    // Importing the generateRandomTitle function
    [DllImport("Dll1.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern IntPtr generateRandomTitle();

    // Importing the getProcId function
    [DllImport("Dll1.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern uint getProcId([MarshalAs(UnmanagedType.LPWStr)] string procName);

    // Importing the injectDll function
    [DllImport("Dll1.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool injectDll([MarshalAs(UnmanagedType.LPWStr)] string dllPath, [MarshalAs(UnmanagedType.LPWStr)] string procName);

    // Helper method to convert the result from generateRandomTitle
    public static string GenerateRandomTitle()
    {
        IntPtr ptr = generateRandomTitle();
        return Marshal.PtrToStringUni(ptr);
    }
}
