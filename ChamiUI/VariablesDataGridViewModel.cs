using ChamiUI.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI
{
    public class VariablesDataGridViewModel : TabbedControlViewModel
    {
        public VariablesDataGridViewModel(ViewModelBase viewModel)
        {
            Control = new VariablesDataGrid(viewModel);
            IsLocked = null;
            TabTitle = ChamiUIStrings.VariablesTabItem_Header;
        }
    }
}