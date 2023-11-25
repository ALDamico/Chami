using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.NewTemplateWindow
{
    public partial class NewTemplateWindow
    {
        public NewTemplateWindow(NewTemplateWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}