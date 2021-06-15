using Microsoft.Win32;

namespace ChamiUI.BusinessLayer.Factories
{
    public static class OpenFileDialogFactory
    {
        public static OpenFileDialog GetOpenFileDialog(string extensions)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = extensions;
            openFileDialog.CheckPathExists = true;
            openFileDialog.CheckFileExists = true;
            return openFileDialog;
        }
    }
}