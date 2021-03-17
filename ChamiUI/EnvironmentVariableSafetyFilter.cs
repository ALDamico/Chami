using System.Collections.Generic;

namespace ChamiUI
{
    public static class EnvironmentVariableSafetyFilter
    {
        public static List<string> OsVariables = new List<string>()
        {
            "ALLUSERSPROFILE",
            "APPDATA",
            "CommonProgramFiles",
            "CommonProgramFiles(x86)",
            "%CommonProgramW6432%",
            "%COMPUTERNAME%",
            "%ComSpec%",
            "%HOMEDRIVE%",
            "%HOMEPATH%",
            "%LOCALAPPDATA%",
        };
    }
}