using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.EnvironmentHealth
{
    public partial class EnvironmentHealthWindow : Window
    {
        public EnvironmentHealthWindow()
        {
            InitializeComponent();
        }

        private void EnvironmentHealthWindowCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EnvironmentHealthWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            if (e.NewValue is EnvironmentHealthViewModel viewModel)
            {
                viewModel.InitWindowColumns(SettingsUtils.GetAppSettings());

                var gridView = GetWindowGridView();
                gridView.Columns.Clear();
                foreach (var column in viewModel.WindowColumns)
                {
                   
                    gridView.Columns.Add(column);
                }
            }

            UpdateLayout();
        }

        private GridView GetWindowGridView()
        {
            return WindowListView.View as GridView;
        }
    }
}