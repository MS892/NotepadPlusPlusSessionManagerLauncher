using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

public class NppAutomation
{
    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    public static void SetProfile(Process npp, string letter)
    {
        IntPtr hwnd = npp.MainWindowHandle;

        if (hwnd == IntPtr.Zero)
        {
            Console.WriteLine("Fensterhandle nicht gefunden.");
            return;
        }

        // Bring N++ to front
        SetForegroundWindow(hwnd);

        // Warte kurz, damit Fenster Fokus bekommt
        Thread.Sleep(300);
        
        SendKeys.SendWait("^%+" + letter);

        // Kontextmenü öffnen (simuliere ALT Taste → Alt + Leertaste → geht oft nicht gut)
        // SendKeys.SendWait("%"); // Alt
        // Thread.Sleep(200);
        // SendKeys.SendWait("s"); // „Sessions...“
        // Thread.Sleep(200);
        // SendKeys.SendWait("d"); // „Default“ oder was du brauchst
    }
}
