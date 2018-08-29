using Microsoft.Win32;
using System.IO;


namespace JeonLib.Game
{
    public static class JeonGame
    {
        public static string GetSteamPath()
        {
            return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam",
                       "SteamPath",
                       "null?");
        }
        public static string GetSteamAppsPath()
        {
            return Path.GetFullPath(Path.Combine(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam",
                       "SourceModInstallPath",
                       "null?").ToString(), @"../"));
        }

        public static string GetCSGOPath()
        {
            try
            {
                return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Applications\csgo.exe\shell\open\command",
                           null,
                           "null?").ToString().Replace(@".exe"" ""%1""", "").Replace(@"""", "");
            }
            catch
            {
                return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Valve.Source\shell\open\command",
                             null,
                             "null?").ToString().Replace(@".exe"" ""%1""", "").Replace(@"""", "");
            }
        }
    }
}
