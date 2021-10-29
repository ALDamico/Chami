using ChamiUI.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.PresentationLayer.ViewModels.Interfaces;

namespace ChamiUI
{
    public class VariablesDataGridViewModel : TabbedControlViewModel
    {
        public VariablesDataGridViewModel(IEnvironmentDatagridModel viewModel)
        {
            Control = new VariablesDataGrid(viewModel);
            IsLocked = null;
            TabTitle = ChamiUIStrings.VariablesTabItem_Header;
        }
    }
}