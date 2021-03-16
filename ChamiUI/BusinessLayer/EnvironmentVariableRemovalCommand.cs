using System.Diagnostics;
using ChamiUI.DataLayer.Entities;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentVariableRemovalCommand:IEnvironmentVariableCommand
    {
        public EnvironmentVariableRemovalCommand(EnvironmentVariable variable)
        {
            EnvironmentVariable = variable;
        }
        public EnvironmentVariable EnvironmentVariable { get; set; }
        public void Execute()
        {
            var arguments = $"/C REG delete HKCU\\Environment /F /V {EnvironmentVariable.Name}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
        }
    }
}