using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using ChamiUI.Utils;

namespace ChamiUI.Windows.AboutBox
{
    public partial class AboutBox
    {
        public AboutBox(Window owner)
        {
            Owner = owner;
            InitializeComponent();
            InitializeRuntimeInfo();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InitializeRuntimeInfo()
        {
            ApplicationInfoTextBlock.Text = AppUtils.GetRuntimeInfo();
        }
    }
}