using System.Diagnostics;
using ChamiUI.DataLayer.Entities;
namespace ChamiUI.BusinessLayer
{
    public class EnvironmentVariableApplicationCommand:IEnvironmentVariableCommand
    {
        public EnvironmentVariableApplicationCommand(EnvironmentVariable variable)
        {
            EnvironmentVariable = variable;
        }

        public EnvironmentVariable EnvironmentVariable { get; set; }

        public void Execute()
        {
            var arguments = $"/C SETX {EnvironmentVariable.Name} {EnvironmentVariable.Value}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
        }
    }
}