using ColorDict.Core.Apis;
using Microsoft.Win32;
using System.IO;

namespace ColorDict.Core.Helpers
{
    public static class CursorManager
    {
        private const string CursorsRegistryPath = @"HKEY_CURRENT_USER\Control Panel\Cursors\";
        private const string ArrowRegistryName = "Arrow";
        private const string IBeamRegistryName = "IBeam";
        private const string CrosshairRegistryName = "Crosshair";
        private const string HandRegistryName = "Hand";

        private static string _originalArrowCursorPath;
        private static string _originalIBeamCursorPath;
        private static string _originalCrosshairCursorPath;
        private static string _originalHandCursorPath;

        public static void SetColorPickerCursor()
        {
            BackupOriginalCursors();

            ChangeCursor(_originalCrosshairCursorPath, ArrowRegistryName);
            ChangeCursor(_originalCrosshairCursorPath, IBeamRegistryName);
            ChangeCursor(_originalCrosshairCursorPath, HandRegistryName);
        }

        public static void RestoreOriginalCursors()
        {
            ChangeCursor(_originalArrowCursorPath, ArrowRegistryName);
            ChangeCursor(_originalIBeamCursorPath, IBeamRegistryName);
            ChangeCursor(_originalIBeamCursorPath, HandRegistryName);
        }

        private static void ChangeCursor(string curFile, string cursorRegistryName)
        {
            try
            {
                Registry.SetValue(CursorsRegistryPath, cursorRegistryName, curFile);
                Win32Apis.SystemParametersInfo(SPI_SETCURSORS, 0, new IntPtr(0), SPIF_SENDCHANGE);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Failed to change cursor. CurFile: " + curFile + " cursor registry name: " + cursorRegistryName, ex);
            }
        }

        private static void BackupOriginalCursors()
        {
            if (string.IsNullOrEmpty(_originalArrowCursorPath))
            {
                _originalArrowCursorPath = (string)Registry.GetValue(CursorsRegistryPath, ArrowRegistryName, string.Empty) ?? string.Empty;
            }
            if (string.IsNullOrEmpty(_originalIBeamCursorPath))
            {
                _originalIBeamCursorPath = (string)Registry.GetValue(CursorsRegistryPath, IBeamRegistryName, string.Empty) ?? string.Empty;
            }
            if (string.IsNullOrEmpty(_originalCrosshairCursorPath))
            {
                _originalCrosshairCursorPath = (string)Registry.GetValue(CursorsRegistryPath, CrosshairRegistryName, string.Empty) ?? string.Empty;
            }
            if (string.IsNullOrEmpty(_originalHandCursorPath))
            {
                _originalHandCursorPath = (string)Registry.GetValue(CursorsRegistryPath, HandRegistryName, string.Empty) ?? string.Empty;
            }
        }

        const int SPI_SETCURSORS = 0x0057;
        const int SPIF_SENDCHANGE = 0x02;
    }
}
