using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using RGCommon;
using UnityEngine;

namespace ModdedCore;

public static class ConsoleApi
{
    private const int StdOutputHandle = -11;
    private const int StdErrorHandle = -12;
    
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();
    
    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleTitle(string title);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);
    
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    
    [Flags]
    private enum ConsoleModes : uint
    {
        EnableProcessedInput = 0x0001,
        EnableLineInput = 0x0002,
        EnableEchoInput = 0x0004,
        EnableWindowInput = 0x0008,
        EnableMouseInput = 0x0010,
        EnableInsertMode = 0x0020,
        EnableQuickEditMode = 0x0040,
        EnableExtendedFlags = 0x0080,
        EnableAutoPosition = 0x0100,
        EnableVirtualTerminalInput = 0x0200,

        EnableProcessedOutput = 0x0001,
        EnableWrapAtEolOutput = 0x0002,
        EnableVirtualTerminalProcessing = 0x0004,
        DisableNewlineAutoReturn = 0x0008,
        EnableLvbGridWorldwide = 0x0010
    }

    internal static void SetupConsole()
    {
        AllocConsole();
        
        if (GetConsoleWindow() == IntPtr.Zero) return;
        var editionText = MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.SteamVR
            ? string.Empty
            : " PC Edition";
        SetConsoleTitle($"Demeo{editionText} v{VersionInformationExt.MajorMinorVersion}");
            
        var stdOutPtr = GetStdHandle(StdOutputHandle);
        GetConsoleMode(stdOutPtr, out var mode);
        mode |= (uint) ConsoleModes.EnableLineInput | 
                (uint) ConsoleModes.EnableProcessedInput;
        if (!SetConsoleMode(stdOutPtr, mode))
        {
            mode &= ~((uint) ConsoleModes.EnableLineInput | 
                      (uint) ConsoleModes.EnableProcessedInput);
        }
        else
        {
            mode |= (uint)ConsoleModes.EnableVirtualTerminalProcessing;
            if (!SetConsoleMode(stdOutPtr, mode))
            {
                mode &= ~(uint)ConsoleModes.EnableVirtualTerminalProcessing;
            }
        }

        mode |= (uint)ConsoleModes.EnableExtendedFlags;
        mode &= ~((uint)ConsoleModes.EnableMouseInput | (uint)ConsoleModes.EnableWindowInput |
                  (uint)ConsoleModes.EnableInsertMode);
        SetConsoleMode(stdOutPtr, mode);
        
        SetStdHandle(StdOutputHandle, stdOutPtr);
        SetStdHandle(StdErrorHandle, stdOutPtr);

        var outStream = new FileStream(new SafeFileHandle(stdOutPtr, false), FileAccess.Write);
        var streamWriter = new StreamWriter(outStream);
        streamWriter.AutoFlush = true;

        Application.logMessageReceivedThreaded += (condition, _, _) => streamWriter.WriteLine(condition);
        Console.SetOut(streamWriter);
        Console.SetError(streamWriter);
        
        Console.WriteLine("===== Console Enabled =====");
    }
}