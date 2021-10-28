using ChamiUI.Controls;
using ChamiUI.Localization;
using NetOffice.ExcelApi;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ConsoleTabViewModel : TabbedControlViewModel
    {
        public ConsoleTabViewModel()
        {
            Control = new ConsoleTextBox();
            TabTitle = ChamiUIStrings.ConsoleTabItem_Header;
            IsLocked = false;
        }
    }
}