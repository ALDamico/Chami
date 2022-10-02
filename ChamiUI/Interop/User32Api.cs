using System;
using System.Runtime.InteropServices;

namespace ChamiUI.Interop;

public static class User32Api
{
    private const string DLL_NAME = "User32.dll";
    
    public  const int SW_RESTORE = 9;
    [DllImport(DLL_NAME)]
    internal static extern bool IsIconic(IntPtr hWnd);

    [DllImport(DLL_NAME)]
    internal static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport(DLL_NAME)]
    internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}