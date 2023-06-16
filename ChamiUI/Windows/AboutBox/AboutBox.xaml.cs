using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ChamiUI.Windows.AboutBox
{
    public partial class AboutBox
    {
        public AboutBox(Window owner)
        {
            Owner = owner;
            InitializeComponent();
            InitializeRuntimeInfo();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private const string NotAvailableString = "N/A";
        private const string NoArguments = "None";

        private void InitializeRuntimeInfo()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var applicationName = NotAvailableString;
            var applicationVersion = NotAvailableString;
            var clrVersion = Environment.Version.ToString();
            var commandLine = Environment.CommandLine;
            var processId = Environment.ProcessId.ToString();
            var arguments = NoArguments;
            // The first element in the command line arguments array is the application itself
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                arguments = string.Join(", ", Environment.GetCommandLineArgs().Skip(1));
            }

            if (entryAssembly != null) 
            {
                applicationName = entryAssembly.GetName().Name;
                var assemblyVersion = entryAssembly.GetName().Version;
                if (assemblyVersion != null)
                {
                    applicationVersion = assemblyVersion.ToString();    
                }
                
            }

            ApplicationInfoTextBlock.Text = $@"Application name: {applicationName}
Application version: {applicationVersion}
CLR information:
CLR version: {clrVersion}
Command line: {commandLine}
Arguments: {arguments}
Process ID: {processId}
";

        }
    }
}