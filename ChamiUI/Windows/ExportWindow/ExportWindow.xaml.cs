using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.Windows.ExportWindow
{
    /// <summary>
    /// Logica di interazione per ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow 
    {
        public ExportWindow(ExportWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
