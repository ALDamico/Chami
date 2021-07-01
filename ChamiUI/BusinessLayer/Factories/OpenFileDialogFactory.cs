using Microsoft.Win32;

namespace ChamiUI.BusinessLayer.Factories
{
    /// <summary>
    /// Static factory class for creating a basic <see cref="OpenFileDialog"/>.
    /// </summary>
    public static class OpenFileDialogFactory
    {
        /// <summary>
        /// Create a new <see cref="OpenFileDialog"/> object.
        /// </summary>
        /// <param name="extensions">The string containing the allowed extensions.</param>
        /// <param name="multiSelect">Determines whether the <see cref="OpenFileDialog"/> allows selecting multiple items or not.</param>
        /// <returns></returns>
        public static OpenFileDialog GetOpenFileDialog(string extensions, bool multiSelect = false)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = multiSelect;
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = extensions;
            openFileDialog.CheckPathExists = true;
            openFileDialog.CheckFileExists = true;
            return openFileDialog;
        }
    }
}