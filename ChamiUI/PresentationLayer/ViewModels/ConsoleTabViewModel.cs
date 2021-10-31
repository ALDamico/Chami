using Chami.Plugins.Contracts.ViewModels;
using ChamiUI.Controls;
using ChamiUI.Localization;

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