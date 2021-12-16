using System;
using System.Windows;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.NewTemplateWindow
{
    public partial class NewTemplateWindow
    {
        public NewTemplateWindow(Window owner)
        {
            Owner = owner;
            _viewmodel = new NewTemplateWindowViewModel();
            DataContext = _viewmodel;
            InitializeComponent();
        }

        private readonly NewTemplateWindowViewModel _viewmodel;

        public event EventHandler<EnvironmentSavedEventArgs> EnvironmentSaved; 

        private void NewEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewmodel.SaveTemplate();
            EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(_viewmodel.Environment));
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}